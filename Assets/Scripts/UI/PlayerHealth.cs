using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charaName;
    [SerializeField] private Slider healthBar;
    [SerializeField] private Image characterProfile;

    //public Image Fill;

    public void UpdateHealth(int newHealth)
    {
        healthBar.value = newHealth;
    }

    public void UpdateEntireUI(int newHealth, string newName, int maxHealth, Sprite profile)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = newHealth;
        charaName.text = newName;
        characterProfile.sprite = profile;
    }

    public void InitializePlayerUI(HeroUnitBase player)
    {
        healthBar.maxValue = player.Stats.Health;
        healthBar.value = player.Stats.Health;
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
