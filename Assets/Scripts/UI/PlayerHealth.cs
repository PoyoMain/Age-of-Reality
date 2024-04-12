using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charaName;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider mpBar;
    [SerializeField] private Image characterProfile;

    //public Image Fill;

    public void UpdateHealth(int newHealth)
    {
        healthBar.value = newHealth;
    }

    public void UpdateMP(int newMP)
    {
        mpBar.value = newMP;
    }

    public void UpdateEntireUI(int newHealth, string newName, int maxHealth, Sprite profile, int newMP = 0, int maxMP = 0)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = newHealth;
        charaName.text = newName;
        characterProfile.sprite = profile;

        if (maxMP > 0)
        {
            mpBar.maxValue = maxMP;
            mpBar.value = newMP;
        }
    }

    public void InitializePlayerUI(HeroUnitBase player)
    {
        healthBar.maxValue = player.Stats.Health;
        healthBar.value = player.Stats.Health;
        mpBar.maxValue = player.Stats.Stamina;
        mpBar.value = player.Stats.Stamina;
        charaName.text = player.data.name;
        characterProfile.sprite = player.data.Profile;
    }

    public void InitializeEnemyUI(EnemyUnitBase enemy)
    {
        healthBar.maxValue = enemy.Stats.Health;
        healthBar.value = enemy.Stats.Health;
        charaName.text = enemy.data.name;
        characterProfile.sprite = enemy.data.Profile;
    }
}
