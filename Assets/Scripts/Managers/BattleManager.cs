using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
                DespawnUnits();
                GameManager.Instance.ChangeGameState(GameState.Overworld);
                break;
            case BattleState.Lose:
                print("Lose");
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
        ChangeState(BattleState.SpawningHeroes);
    }

    private void SpawningHeroesCase()
    {
        PartyUnits = UnitManager.Instance.SpawnHeroes(GameManager.Instance.Party);

        ChangeState(BattleState.SpawningEnemies);
    }

    private void SpawningEnemiesCase()
    {
        EnemyUnits = UnitManager.Instance.SpawnEnemies(GameManager.Instance.enemyHit.team);

        turnOrder.AddRange(PartyUnits);
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

        while (AttackMenu.chosenAttack == null)
        {
            if (battleControls.SelectionUp.triggered) AttackMenu.SelectUp();
            else if (battleControls.SelectionDown.triggered) AttackMenu.SelectDown();
            else if (battleControls.Select.triggered) AttackMenu.Select();
            yield return null;
        }

        AttackMenu.gameObject.SetActive(false);
        pickingEnemy = true;
        selectedEnemy = EnemyUnits[0];
        selectedEnemy.selectIndicator.SetActive(true);

        yield return new WaitForSeconds(0.5f);

        while (pickingEnemy)
        {
            if (battleControls.SelectionUp.triggered) EnemySelect(true);
            else if (battleControls.SelectionDown.triggered) EnemySelect(false);
            else if (battleControls.Select.triggered) pickingEnemy = false;
            yield return null;
        }

        selectedEnemy.selectIndicator.SetActive(false);

        LineGenerator minigame = ResourceStorage.Instance.GetMinigame(Enum.GetName(typeof(MinigameType), AttackMenu.chosenAttack.Minigame));
        minigameManager.SetMinigame(minigame);
        minigameManager.gameObject.SetActive(true);
        //Instantiate(minigame, GridInfo.GridWorldMidPoint, Quaternion.identity);

        while(minigameManager.MinigameRunning)
        {
            yield return null;
        }

        minigameManager.gameObject.SetActive(false);

        turnOrder[0].Attack(AttackMenu.chosenAttack, selectedEnemy, multiplier: AttackMenu.chosenAttack.Stats.multiplier);

        if (selectedEnemy.Stats.Health <= 0)
        {
            turnOrder.Remove(selectedEnemy);
            EnemyUnits.Remove(selectedEnemy);
            Destroy(selectedEnemy.gameObject);
        }

        AttackMenu.gameObject.SetActive(false);
        AttackMenu.chosenAttack = null;

        while (!turnOrder[0].Move(startPos, speed))
        {
            yield return null;
        }

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
        print(chosenTarget.Stats.Health);

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
    }


    /// <summary>
    /// Despawns units at the end of battle 
    /// </summary>
    void DespawnUnits()
    {
        for (int i = 0; i < EnemyUnits.Count; i++)
        {
            Destroy(EnemyUnits[i].gameObject);
        }
        EnemyUnits.Clear();

        for (int i = 0; i < PartyUnits.Count; i++)
        {
            Destroy(PartyUnits[i].gameObject);
        }
        PartyUnits.Clear();
        turnOrder.Clear();
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
    PickingEnemy,
    EnemyTurn,
    Win,
    Lose
}
