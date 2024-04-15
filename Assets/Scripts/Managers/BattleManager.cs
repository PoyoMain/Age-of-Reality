using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(Animator))]
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
    private bool pickingEnemy;

    private HeroUnitBase selectedPlayer;
    private bool pickingPlayer;

    [SerializeField] private PlayerHealth playerHealthUI;
    [SerializeField] private PlayerHealth enemyHealthUI;

    [Space(15f)]
    [SerializeField] private XPWindow xpWindow;
    [SerializeField] private ScriptableLevelSystem levelSystem;

    [Space(15f)] 
    [SerializeField] private List<ScriptableItem> givenItems;

    private Animator _anim;

    void Awake()
    {
        playerControls = new PlayerControls();
        battleControls = playerControls.BattleControls;

        _anim = GetComponent<Animator>();
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
        givenItems.Clear();
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
        enemyHealthUI.InitializeEnemyUI(EnemyUnits[0]);

        //List<UnitBase> temp = turnOrder;
        //for (int i = 0; i <= turnOrder.Count - 1; i++)
        //{
        //    for (int j = 0; i <= turnOrder.Count - 1; j++)
        //    {
        //        if (!(j == turnOrder.Count))
        //        {
        //            if (CompareSpeed(temp[i], temp[j]))
        //            {
        //                UnitBase tempUnit = temp[i];
        //                temp[i] = temp[j];
        //                temp[j] = tempUnit;
        //            }
        //        }
        //    }
        //}

        ChangeState(BattleState.HeroTurn);
    }

    bool CompareSpeed(UnitBase a, UnitBase b)
    {
        if (a is HeroUnitBase && b is HeroUnitBase)
        {
            if (b is HeroUnitBase)
            {
                if ((a as HeroUnitBase).data.BaseStats.Speed >= (b as HeroUnitBase).data.BaseStats.Speed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((a as HeroUnitBase).data.BaseStats.Speed >= (b as EnemyUnitBase).data.BaseStats.Speed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }

        if (a is EnemyUnitBase && b is EnemyUnitBase)
        {
            if (b is EnemyUnitBase)
            {
                if ((a as EnemyUnitBase).data.BaseStats.Speed >= (b as EnemyUnitBase).data.BaseStats.Speed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if ((a as EnemyUnitBase).data.BaseStats.Speed >= (b as HeroUnitBase).data.BaseStats.Speed)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        return false;
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
        HeroUnitBase currentHero = turnOrder[0] as HeroUnitBase;

        Vector3 startPos = currentHero.transform.position;

        while (!currentHero.Move(GridInfo.GridWorldMidPoint, speed * 2))
        {
            yield return null;
        }

        AttackMenu.gameObject.SetActive(true);
        AttackMenu.SetCurrentUnit(CurrentPartyMemberActive);

        while (AttackMenu.chosenAttack == null && AttackMenu.chosenItem == null && !AttackMenu.flee)
        {
            yield return null;
        }

        if (AttackMenu.chosenAttack != null)
        {
            AttackMenu.ResetAnimator();
            yield return new WaitForSeconds(0.5f);
            AttackMenu.gameObject.SetActive(false);
            pickingEnemy = false;

            pickingEnemy = true;
            EnemySelected(EnemyUnits[0]);

            while (pickingEnemy)
            {
                yield return null;
            }
            pickingEnemy = false;

            LineMinigameBase minigame = AttackMenu.chosenAttack.Minigames[UnityEngine.Random.Range(0, AttackMenu.chosenAttack.Minigames.Length)];
            minigameManager.SetMinigame(minigame, AttackMenu.chosenAttack.SecondsToComplete);
            minigameManager.gameObject.SetActive(true);
            //Instantiate(minigame, GridInfo.GridWorldMidPoint, Quaternion.identity);

            while (minigameManager.MinigameRunning)
            {
                yield return null;
            }

            minigameManager.gameObject.SetActive(false);

            int damage = currentHero.Attack(AttackMenu.chosenAttack, selectedEnemy, accuracy: minigameManager.Accuracy);

            _anim.SetInteger("Damage", damage);

            while (!selectedEnemy.FullAttackDone)
            {
                yield return null;
            }

            currentHero.AttackStateReset();
            selectedEnemy.AttackStateReset();

            playerHealthUI.UpdateMP(currentHero.Stats.Stamina);
            enemyHealthUI.UpdateHealth(selectedEnemy.Stats.Health);


            if (selectedEnemy.Stats.Health <= 0)
            {
                currentHero.data.IncreaseXP(selectedEnemy.data.BaseStats.XP);

                foreach (ItemDrop item in selectedEnemy.data.droppableItems)
                {
                    if (UnityEngine.Random.Range(0, 1.0f) <= item.ChanceToDrop)
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
            selectedEnemy = null;
        }
        else if (AttackMenu.chosenItem != null)
        {
            AttackMenu.ResetAnimator();
            yield return new WaitForSeconds(0.5f);
            AttackMenu.gameObject.SetActive(false);
            pickingPlayer = false;
            PlayerSelected(PartyUnits[0]);

            yield return new WaitForSeconds(0.5f);

            pickingPlayer = true;
            while (pickingPlayer)
            {
                yield return null;
            }
            pickingPlayer = false;

            switch (AttackMenu.chosenItem.Effect)
            {
                case ItemEffect.Heal:
                    selectedPlayer.SetEffects(AttackMenu.chosenItem);
                    playerHealthUI.UpdateHealth(currentHero.Stats.Health);
                    GameManager.Instance.ItemInventory.UseItem(AttackMenu.chosenItem);
                    break;
                case ItemEffect.AP:
                    selectedPlayer.SetEffects(AttackMenu.chosenItem);
                    playerHealthUI.UpdateMP(currentHero.Stats.Stamina);
                    GameManager.Instance.ItemInventory.UseItem(AttackMenu.chosenItem);
                    break;
                case ItemEffect.AttackBoost:
                    break;
                case ItemEffect.Evasiveness:
                    break;
            }

            AttackMenu.gameObject.SetActive(false);
            AttackMenu.chosenItem = null;
            selectedPlayer = null;
        }
        else if (AttackMenu.flee)
        {
            AttackMenu.ResetAnimator();

            while (AttackMenu.flee)
            {
                yield return null;
            }

            AttackMenu.gameObject.SetActive(false);
            AttackMenu.flee = false;
            ChangeState(BattleState.Flee);
            yield break;
        }

        

        while (!currentHero.Move(startPos, speed * 2))
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
        EnemyUnitBase enemy = turnOrder[0] as EnemyUnitBase;

        Vector3 startPos = enemy.transform.position;

        while (!enemy.Move(GridInfo.GridWorldMidPoint, speed * 2))
        {
            yield return null;
        }
        
        HeroUnitBase chosenTarget = PartyUnits[UnityEngine.Random.Range(0, PartyUnits.Count)];

        int damage = enemy.Attack(chosenTarget);
        

        while (!chosenTarget.FullAttackDone)
        {
            yield return null;
        }

        

        enemy.AttackStateReset();
        chosenTarget.AttackStateReset();

        playerHealthUI.UpdateHealth(chosenTarget.Stats.Health);

        if (chosenTarget.Stats.Health <= 0)
        {
            turnOrder.Remove(chosenTarget);
            PartyUnits.Remove(chosenTarget);
            Destroy(chosenTarget.gameObject);
        }

        while (!enemy.Move(startPos, speed * 2))
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
        AudioManager.Instance.PlayBattleSFX(soundType: BattleSounds.Victory);
        xpWindow.gameObject.SetActive(true);
        xpWindow.ActivateWinVisual(givenItems);

        if (givenItems.Count > 0)
        {
            AudioManager.Instance.PlayBattleSFX(soundType: BattleSounds.Item);
        }

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
                xpWindow.SetXPStats(heroUnit.data.name, heroUnit.data.Ability == Ability.Magic ,prevData, currentData, newAttacks);
                xpWindow.ActivateXPVisual();
                AudioManager.Instance.PlayBattleSFX(soundType: BattleSounds.LevelUp);

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

    void EnemySelected(EnemyUnitBase enemy)
    {
        if (AttackMenu.chosenAttack == null || pickingEnemy == false) return;

        if (enemy == selectedEnemy)
        {
            pickingEnemy = false;
            selectedEnemy.IsSelected = false;
        }
        else
        {
            if (selectedEnemy != null) selectedEnemy.IsSelected = false;
            selectedEnemy = enemy;
            selectedEnemy.IsSelected = true;
            enemyHealthUI.UpdateEntireUI(selectedEnemy.Stats.Health, selectedEnemy.data.name, selectedEnemy.MaxHealth, selectedEnemy.data.Profile);
            enemyHealthUI.gameObject.SetActive(true);
        }
    }

    void PlayerSelected(HeroUnitBase hero)
    {
        if (AttackMenu.chosenItem == null || pickingPlayer == false) return;

        if (hero == selectedPlayer)
        {
            pickingPlayer = false;
            selectedPlayer.IsSelected = false;
        }
        else
        {
            if (selectedPlayer != null) selectedPlayer.IsSelected = false;
            selectedPlayer = hero;
            selectedPlayer.IsSelected = true;
            playerHealthUI.UpdateEntireUI(selectedPlayer.Stats.Health, selectedPlayer.data.name, selectedPlayer.MaxHealth, selectedPlayer.data.Profile, selectedPlayer.Stats.Stamina, selectedPlayer.MaxAP);
            playerHealthUI.gameObject.SetActive(true);
        }
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

    public void ShakeCamera()
    {
        _anim.SetTrigger("Shake Cam");

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
