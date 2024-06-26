using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [Header("General Settings")]
    [SerializeField] private PlayerController player; // The player's overworld character
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private Camera overworldCamera;
    [SerializeField] private Camera battleCamera;

    [Space(15)]
    [Header("Audio")]
    [SerializeField] private float audioFadeOutTime;
    private AudioSource overworldAudioSource;
    private float overworldAudioStartVolume;
    private AudioSource battleAudioSource;
    private AudioBG battleAudioManager;
    private float battleAudioStartVolume;
    [HideInInspector] public bool modeTransitioning;

    private GameState State;

    public List<ScriptableHero> Party
    {
        get { return player.Party; }
    }
    [HideInInspector] public Enemy enemyHit; // The enemy hit by the player

    [Space(15)]
    [Header("Inventory")]
    public ScriptableInventory ItemInventory;

    [Space(15)]
    [Header("Dialogue")]
    public Dialogue dialogueBox;

    [HideInInspector] public int perfectMinigameCount;
    [HideInInspector] public int enemiesDefeated;

    protected override void Awake()
    {
        base.Awake();

        overworldAudioSource = overworldCamera.gameObject.GetComponent<AudioSource>();
        overworldAudioStartVolume = overworldAudioSource.volume;

        battleAudioManager =  battleCamera.gameObject.GetComponent<AudioBG>();
        battleAudioSource = battleCamera.gameObject.GetComponent<AudioSource>();
        battleAudioStartVolume = battleAudioSource.volume;
    }

    // Method to change the state of the game
    public void ChangeGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Overworld:
                EndBattle();
                break;
            case GameState.Battle:
                StartBattle();
                break;
        }
    }

    // Starts a battle. 
    void StartBattle()
    {
        StartCoroutine(StartBattleCoroutine());
    }

    IEnumerator StartBattleCoroutine()
    {
        modeTransitioning = true;

        enemyHit.Freeze();
        player.Freeze();

        if (!(enemyHit.Line == null || enemyHit.Line.Line == string.Empty))
        {
            dialogueBox.Init(enemyHit.Line, enemyHit.type);

            while (dialogueBox.PlayingDialogue)
            {
                yield return null;
            }
        }

        while (overworldAudioSource.volume > 0.1)
        {
            overworldAudioSource.volume -=  Time.deltaTime / audioFadeOutTime;
            yield return null;
        }

        modeTransitioning = false;

        if (enemyHit.isBoss) battleAudioManager.playAltTracks = true;
        else battleAudioManager.playAltTracks = false;

        battleManager.gameObject.SetActive(true);
        battleManager.enabled = true; // activates the battle manager

        overworldCamera.gameObject.SetActive(false); // deactivates the overworld camera
        overworldAudioSource.volume = overworldAudioStartVolume;


    }

    // Called at the end of a battle
    void EndBattle()
    {
        StartCoroutine(EndBattleCoroutine());  
    }

    IEnumerator EndBattleCoroutine()
    {
        modeTransitioning = true;

        while (battleAudioSource.volume > 0.1)
        {
            battleAudioSource.volume -= Time.deltaTime / audioFadeOutTime;
            yield return null;
        }

        modeTransitioning = false;

        battleManager.gameObject.SetActive(false);
        battleManager.enabled = false; // Deactivates the battle manager
        battleAudioSource.volume = battleAudioStartVolume;

        overworldCamera.gameObject.SetActive(true); // Activates the overworld camera

        player.UnFreeze();
        enemyHit.UnFreeze();

        if (enemyHit.isBoss && battleManager.State != BattleState.Flee)
        {
            //SceneManager.LoadScene("WinScene");
        }

        if (battleManager.State == BattleState.Win)
        {
            Destroy(enemyHit.gameObject); // If the player wins the battle, destroy the enemy
        }
        else
        {
            enemyHit.MakeInvincible(); // If the player flees, make the enemy invincible for a few seconds
        }

        enemyHit = null;

    }

    public void EnemyHit(Enemy enemy)
    {
        if (enemyHit != null) return;

        enemyHit = enemy;
        ChangeGameState(GameState.Battle);
    }

    public Camera GetBattleCamera()
    {
        return battleCamera;
    }

    public Camera GetOverworldCamera()
    {
        return overworldCamera;
    }

    private void FixedUpdate()
    {
        // If in overworld mode, let the player move
        if (State == GameState.Overworld)
        {
            player.HandleUpdate();
        }
    }

    private void OnDisable()
    {
        ItemInventory.Inventory.Clear();
        perfectMinigameCount = 0;
    }
}

// States of the game
public enum GameState
{
    Overworld,
    Battle,
}