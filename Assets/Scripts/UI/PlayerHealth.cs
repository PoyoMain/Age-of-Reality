using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{

    public float maxHealth = 4;
    public float currentHealth;
    public Image Fill;

    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        Fill.fillAmount = Mathf.Clamp(currentHealth / maxHealth, 0, 1);
    }
}
