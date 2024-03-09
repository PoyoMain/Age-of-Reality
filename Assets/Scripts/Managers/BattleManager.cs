using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    public static event Action<BattleState> OnBeforeStateChanged; // Event that happens before a state change
    public static event Action<BattleState> OnAfterStateChanged; // Event that happens after a state change

    [SerializeField] private MinigameManager minigameManager; // Manages the minigame

    [SerializeField] private float speed = 2; // Speed the units move on the board
    [SerializeField] private ActionSelectMenu AttackMenu; // Battle menu that controls which action the player will take

    public BattleState State { get; private set; }

    private PlayerControls playerControls;
    private PlayerControls.BattleControlsActions battleControls;

    private List<HeroUnitBase> PartyUnits;  // List of Hero Units
    private List<EnemyUnitBase> EnemyUnits; // List of Enemy Units
    private readonly List<UnitBase> turnOrder = new(); // The list of active units in the order they attack in
    public HeroUnitBase CurrentPartyMemberActive
    {
        get { return turnOrder[0] as HeroUnitBase; }
        private set { }
    }

    // Variables for picking an enemy to attck
    private EnemyUnitBase selectedEnemy;
    private int enemyIndex;
    private bool pickingEnemy;

    private HeroUnitBase selectedPlayer;
    private int playerIndex;
    private bool pickingPlayer;

    [SerializeField] private PlayerHealth playerHealthUI;
    [SerializeField] private PlayerHealth enemyHealthUI;

    [Space(15f)]
    [SerializeField] private XPWindow xpWindow;
    [SerializeField] private ScriptableLevelSystem levelSystem;

    [Space(15f)] 
    [SerializeField] private List<ScriptableItem> givenItems;

    void Awake()
    {
        playerControls = new PlayerControls();
        battleControls = playerControls.BattleControls;
    }

    // Method to change the state of the battle
    public void ChangeState(BattleState newState)
    {
        OnBeforeStateChanged?.Invoke(newState);

        State = newState;

        switch (newState)
        {
            case BattleState.Starting:
                StartingCase();
                break;
            case BattleState.SpawningHeroes:
                SpawningHeroesCase();
                break;
            case BattleState.SpawningEnemies:
                SpawningEnemiesCase();
                break;
            case BattleState.HeroTurn:
                HeroTurnCase();
                break;
            case BattleState.EnemyTurn:
                EnemyTurnCase();
                break;
            case BattleState.Win:
                print("Win");
                WinCase();
                break;
            case BattleState.Lose:
                print("Lose");
                DespawnUnits();
                GameManager.Instance.ChangeGameState(GameState.Overworld);
                SceneManager.LoadScene("DeathScene");
                break;
            case BattleState.Flee:
                print("Flee");
                DespawnUnits();
                GameManager.Instance.ChangeGameState(GameState.Overworld);
                break;
            default:
                Debug.LogError("No case for " + newState + " game state");
                break;
        }

        OnAfterStateChanged?.Invoke(newState);
    }

    private void StartingCase()
    {
        turnOrder.Clear();
        ChangeState(BattleState.SpawningHeroes);
    }

    private void SpawningHeroesCase()
    {
        PartyUnits = UnitManager.Instance.SpawnHeroes(GameManager.Instance.Party);
        turnOrder.AddRange(PartyUnits);
        playerHealthUI.InitializePlayerUI(PartyUnits[0]);

        ChangeState(BattleState.SpawningEnemies);
    }

    private void SpawningEnemiesCase()
    {
        EnemyUnits = UnitManager.Instance.SpawnEnemies(GameManager.Instance.enemyHit.team);
        turnOrder.AddRange(EnemyUnits);
        //turnOrder.Sort((a, b) => a.Stats.Speed.CompareTo(b.Stats.Speed));

        ChangeState(BattleState.HeroTurn);
    }

    /// <summary>
    /// Run players turn
    /// </summary>
    private void HeroTurnCase()
    {
        if (EnemyUnits.Count <= 0)
        {
            ChangeState(BattleState.Win);
            return;
        }

        foreach (HeroUnitBase unit in PartyUnits) 
        {
            unit.DoEffects();
        }

        StartCoroutine(HeroTurnCoroutine());        
    }

    private IEnumerator HeroTurnCoroutine()
    {
        Vector3 startPos = turnOrder[0].transform.position;

        while (!turnOrder[0].Move(GridInfo.GridWorldMidPoint, speed))
        {
            yield return null;
        }

        AttackMenu.gameObject.SetActive(true);
        AttackMenu.SetCurrentUnit(CurrentPartyMemberActive);

        while (AttackMenu.chosenAttack == null && AttackMenu.chosenItem == null && !AttackMenu.flee)
        {
            if (battleControls.SelectionUp.triggered) AttackMenu.SelectUp();
            else if (battleControls.SelectionDown.triggered) AttackMenu.SelectDown();
            else if (battleControls.Select.triggered) AttackMenu.Select();
            yield return null;
        }

        if (AttackMenu.chosenAttack != null)
        {
            AttackMenu.gameObject.SetActive(false);
            pickingEnemy = true;
            selectedEnemy = EnemyUnits[0];
            selectedEnemy.selectIndicator.SetActive(true);
            enemyHealthUI.InitializeEnemyUI(selectedEnemy);
            enemyHealthUI.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            while (pickingEnemy)
            {
                if (battleControls.SelectionUp.triggered) EnemySelect(true);
                else if (battleControls.SelectionDown.triggered) EnemySelect(false);
                else if (battleControls.Select.triggered) pickingEnemy = false;
                yield return null;
            }

            selectedEnemy.selectIndicator.SetActive(false);

            LineMinigameBase minigame = AttackMenu.chosenAttack.Minigame;
            minigameManager.SetMinigame(minigame, AttackMenu.chosenAttack.SecondsToComplete);
            minigameManager.gameObject.SetActive(true);
            //Instantiate(minigame, GridInfo.GridWorldMidPoint, Quaternion.identity);

            while (minigameManager.MinigameRunning)
            {
                yield return null;
            }

            minigameManager.gameObject.SetActive(false);

            turnOrder[0].Attack(AttackMenu.chosenAttack, selectedEnemy, multiplier: AttackMenu.chosenAttack.Stats.multiplier);
            enemyHealthUI.UpdateHealth(selectedEnemy.Stats.Health);

            if (selectedEnemy.Stats.Health <= 0)
            {
                HeroUnitBase currentHero = turnOrder[0] as HeroUnitBase;
                currentHero.data.IncreaseXP(selectedEnemy.data.BaseStats.XP);

                foreach (ItemDrop item in selectedEnemy.data.droppableItems)
                {
                    if (UnityEngine.Random.Range(0, 1) <= item.ChanceToDrop)
                    {
                        givenItems.Add(item.Item);
                    }
                }

                turnOrder.Remove(selectedEnemy);
                EnemyUnits.Remove(selectedEnemy);
                Destroy(selectedEnemy.gameObject);

                GameManager.Instance.enemiesDefeated++;
            }

            AttackMenu.gameObject.SetActive(false);
            AttackMenu.chosenAttack = null;
        }
        else if (AttackMenu.chosenItem != null)
        {
            AttackMenu.gameObject.SetActive(false);
            pickingPlayer = true;
            selectedPlayer = PartyUnits[0];
            selectedPlayer.selectIndicator.SetActive(true);
            playerHealthUI.InitializePlayerUI(selectedPlayer);

            yield return new WaitForSeconds(0.5f);

            while (pickingPlayer)
            {
                print("Hi");
                if (battleControls.SelectionUp.triggered) PlayerSelect(true);
                else if (battleControls.SelectionDown.triggered) PlayerSelect(false);
                else if (battleControls.Select.triggered) pickingPlayer = false;
                yield return null;
            }

            selectedPlayer.selectIndicator.SetActive(false);

            switch (AttackMenu.chosenItem.Effect)
            {
                case ItemEffect.Heal:
                    selectedPlayer.SetEffects(AttackMenu.chosenItem);
                    HeroUnitBase playerUnit = turnOrder[0] as HeroUnitBase;
                    playerHealthUI.UpdateHealth(playerUnit.Stats.Health);
                    GameManager.Instance.ItemInventory.UseItem(AttackMenu.chosenItem);
                    break;
                case ItemEffect.AttackBoost:
                    break;
                case ItemEffect.Evasiveness:
                    break;
            }

            AttackMenu.gameObject.SetActive(false);
            AttackMenu.chosenItem = null;
        }
        else if (AttackMenu.flee)
        {
            AttackMenu.gameObject.SetActive(false);
            ChangeState(BattleState.Flee);
            yield break;
        }


        while (!turnOrder[0].Move(startPos, speed))
        {
            yield return null;
        }

        enemyHealthUI.gameObject.SetActive(false);
        NextTurn();
    }

    /// <summary>
    /// Run enemy's turn
    /// </summary>
    private void EnemyTurnCase()
    {
        if (PartyUnits.Count <= 0)
        {
            ChangeState(BattleState.Lose);
            return;
        }

        foreach (EnemyUnitBase unit in EnemyUnits)
        {
            unit.DoEffects();
        }

        StartCoroutine(EnemyTurnCoroutine());
    }

    private IEnumerator EnemyTurnCoroutine()
    {
        Vector3 startPos = turnOrder[0].transform.position;

        while (!turnOrder[0].Move(GridInfo.GridWorldMidPoint, speed))
        {
            yield return null;
        }

        EnemyUnitBase enemy = turnOrder[0] as EnemyUnitBase;
        ScriptableAttack chosenAttack = enemy.data.attacks[UnityEngine.Random.Range(0, enemy.data.attacks.Count)];
        HeroUnitBase chosenTarget = PartyUnits[UnityEngine.Random.Range(0, PartyUnits.Count)];

        turnOrder[0].Attack(chosenAttack, chosenTarget);
        playerHealthUI.UpdateHealth(chosenTarget.Stats.Health);

        if (chosenTarget.Stats.Health <= 0)
        {
            turnOrder.Remove(chosenTarget);
            PartyUnits.Remove(chosenTarget);
            Destroy(chosenTarget.gameObject);
        }

        while (!turnOrder[0].Move(startPos, speed))
        {
            yield return null;
        }



        NextTurn();
        yield return null;
    }

    private void WinCase()
    {
        StartCoroutine(WinCoroutine());
    }

    private IEnumerator WinCoroutine()
    {
        DespawnUnits(true);
        xpWindow.gameObject.SetActive(true);
        xpWindow.ActivateWinVisual(givenItems);

        foreach (var item in givenItems)
        {
            GameManager.Instance.ItemInventory.AddToInventory(item);
        }

        while (!Input.GetMouseButtonDown(0))
        {
            yield return null;
        }

        foreach (HeroUnitBase heroUnit in PartyUnits)
        {
            bool canLevelUp = false;

            HeroStats prevData = heroUnit.data.BaseStats;
            List<ScriptableAttack> newAttacks = new();
            

            while (heroUnit.data.LevelUp(levelSystem, out ScriptableAttack newAttack))
            {
                canLevelUp = true;
                if (newAttack != null)
                {
                    newAttacks.Add(newAttack);
                }
                yield return null;
            }
            HeroStats currentData = heroUnit.data.BaseStats;

            if (canLevelUp)
            {
                xpWindow.SetXPStats(heroUnit.data.name, prevData, currentData, newAttacks);
                xpWindow.ActivateXPVisual();

                while (!Input.GetMouseButtonDown(0))
                {
                    yield return null;
                }
            }   
        }

        xpWindow.gameObject.SetActive(false);
        DespawnUnits();
        GameManager.Instance.ChangeGameState(GameState.Overworld);
    }

    /// <summary>
    /// Puts the current unit at the back of the list and determines which side's turn it is
    /// </summary>
    public void NextTurn()
    {
        UnitBase prevTurn = turnOrder[0];
        turnOrder.RemoveAt(0);
        turnOrder.Add(prevTurn);

        UnitBase nextTurn = turnOrder[0];

        StopAllCoroutines();
        if (nextTurn.gameObject.TryGetComponent<HeroUnitBase>(out _)) 
        {
            ChangeState(BattleState.HeroTurn);
        }
        else
        {
            ChangeState(BattleState.EnemyTurn);
        }

    }

    /// <summary>
    /// Select an enemy
    /// </summary>
    /// <param name="up">The direction of the new selection</param>
    void EnemySelect(bool up)
    {
        enemyIndex = up ? (enemyIndex + 1) % EnemyUnits.Count : (enemyIndex - 1) < 0 ? EnemyUnits.Count - 1 : enemyIndex - 1;
        selectedEnemy = EnemyUnits[enemyIndex];

        foreach (EnemyUnitBase enemy in EnemyUnits)
        {
            enemy.selectIndicator.SetActive(false);
        }

        selectedEnemy.selectIndicator.SetActive(true);
        enemyHealthUI.UpdateEntireUI(selectedEnemy.Stats.Health, selectedEnemy.data.name);
    }

    /// <summary>
    /// Select an player
    /// </summary>
    /// <param name="up">The direction of the new selection</param>
    void PlayerSelect(bool up)
    {
        playerIndex = up ? (playerIndex + 1) % PartyUnits.Count : (playerIndex - 1) < 0 ? PartyUnits.Count - 1 : playerIndex - 1;
        selectedPlayer = PartyUnits[playerIndex];

        foreach (HeroUnitBase player in PartyUnits)
        {
            player.selectIndicator.SetActive(false);
        }

        selectedPlayer.selectIndicator.SetActive(true);
        playerHealthUI.UpdateEntireUI(selectedPlayer.Stats.Health, selectedPlayer.data.name);
    }


    /// <summary>
    /// Despawns units at the end of battle 
    /// </summary>
    void DespawnUnits(bool keepPlayers = false)
    {
        for (int i = 0; i < EnemyUnits.Count; i++)
        {
            turnOrder.Remove(EnemyUnits[i]);
            Destroy(EnemyUnits[i].gameObject);
        }
        EnemyUnits.Clear();

        if (!keepPlayers)
        {
            for (int i = 0; i < PartyUnits.Count; i++)
            {
                turnOrder.Remove(PartyUnits[i]);
                Destroy(PartyUnits[i].gameObject);
            }
            PartyUnits.Clear();
        }
    }

    private void OnEnable()
    {
        battleControls.Enable();
        ChangeState(BattleState.Starting);
    }

    private void OnDisable()
    {
        battleControls.Disable();
    }
}

// Battle States
public enum BattleState
{ 
    Starting,
    SpawningHeroes,
    SpawningEnemies,
    HeroTurn,
    EnemyTurn,
    Win,
    Lose,
    Flee,
}
