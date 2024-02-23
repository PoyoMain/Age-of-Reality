using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NPCDecision : MonoBehaviour
{
    [SerializeField] private ScriptableHero player;

    public void SetPlayerMelee(bool choice)
    {
        if (choice) player.Ability = Ability.Melee;
        else player.Ability = Ability.Magic;

        SceneManager.LoadScene("OverworldScene");
    }
}
