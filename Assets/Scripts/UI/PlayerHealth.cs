using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI charaName;
    [SerializeField] private Slider healthBar;

    private float maxHealth = 4;
    private float currentHealth;
    //public Image Fill;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //Fill.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
    }

    public void UpdateHealth(int newHealth)
    {
        healthBar.value = newHealth;
    }

    public void UpdateEntireUI(int newHealth, string newName)
    {
        healthBar.value = newHealth;
        charaName.text = newName;
    }

    public void InitializePlayerUI(HeroUnitBase player)
    {
        healthBar.maxValue = player.Stats.Health;
        healthBar.value = player.Stats.Health;
        charaName.text = player.data.name;
    }

    public void InitializeEnemyUI(EnemyUnitBase enemy)
    {
        healthBar.maxValue = enemy.Stats.Health;
        healthBar.value = enemy.Stats.Health;
        charaName.text = enemy.data.name;
    }
}
