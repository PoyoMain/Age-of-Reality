using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private PlayerController player; // The player's overworld character
    [SerializeField] private BattleManager battleManager;
    [SerializeField] private Camera overworldCamera;
    [SerializeField] private Camera battleCamera;
    [SerializeField] private LineGenerator minigame;

    private GameState State;

    public List<ScriptableHero> Party
    {
        get { return player.Party; }
    }
    [HideInInspector] public Enemy enemyHit; // The enemy hit by the player

    protected override void Awake()
    {
        base.Awake();
    }

    //private void Start()
    //{
    //    LineGenerator minigame = ResourceStorage.Instance.GetMinigame(Enum.GetName(typeof(MinigameType), MinigameType.Magic_Triangle));
    //    Instantiate(minigame, player.transform.position, Quaternion.identity);
    //}

    // Method to change the state of the game
    public void ChangeGameState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.Overworld:
                EndBattle();
                break;
            case GameState.DuringBattle:
                StartBattle();
                break;
        }
    }

    // Starts a battle. 
    void StartBattle()
    {
        enemyHit.Freeze();
        player.Freeze();
        //DontDestroyOnLoad(this.enemyHit.gameObject);
        //SceneManager.LoadScene("BattleScene");

        battleManager.gameObject.SetActive(true);
        battleManager.enabled = true; // activates the battle manager
        overworldCamera.gameObject.SetActive(false); // deactivates the overworld camera
    }

    // Called at the end of a battle
    void EndBattle()
    {
        //SceneManager.LoadScene("OverworldScene");

        battleManager.gameObject.SetActive(false);
        battleManager.enabled = false; // Deactivates the battle manager
        overworldCamera.gameObject.SetActive(true); // Activates the overworld camera

        if (battleManager.State == BattleState.Win)
        {
            Destroy(enemyHit.gameObject); // If the player wins the battle, destroy the enemy
        }
        else
        {
            enemyHit.MakeInvincible(); // If the player loses, make the enemy invincible for a few seconds
        }
    }

    public void EnemyHit(Enemy enemy)
    {
        enemyHit = enemy;
        ChangeGameState(GameState.DuringBattle);
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
}

// States of the game
public enum GameState
{
    Overworld,
    DuringBattle,
}