using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] private ScriptableHero hero;

    [SerializeField] private Image backdrop;
    [SerializeField] private Sprite magicBG;
    [SerializeField] private Sprite meleeBG;

    private void Awake()
    {
        switch (hero.Ability)
        {
            case Ability.Melee:
                backdrop.sprite = meleeBG;
                break;
            case Ability.Magic:
                backdrop.sprite = magicBG;
                break;
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene("OverworldScene");
    }
}
