using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XPWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI staminaText;
    [Space(15f)]
    [SerializeField] private TextMeshProUGUI playerName;
    [Space(15f)]
    [SerializeField] private TextMeshProUGUI levelAmount;
    [Space(15f)]
    [SerializeField] private TextMeshProUGUI healthAmount;
    [SerializeField] private TextMeshProUGUI defenseAmount;
    [SerializeField] private TextMeshProUGUI speedAmount;
    [SerializeField] private TextMeshProUGUI strengthAmount;
    [SerializeField] private TextMeshProUGUI staminaAmount;
    [Space(15f)]
    [SerializeField] private GameObject newAttackVisual;
    [SerializeField] private TextMeshProUGUI newAttacksText;
    [Space(15f)]
    [SerializeField] private GameObject itemVisual;
    [SerializeField] private TextMeshProUGUI itemText;
    [Space(15f)]
    [SerializeField] private GameObject winVisual;
    [SerializeField] private GameObject XPVisual;

    private void Awake()
    {
        winVisual.SetActive(false);
        itemVisual.SetActive(false);
        XPVisual.SetActive(false);
    }

    public void ActivateWinVisual(List<ScriptableItem> drops)
    {
        winVisual.SetActive(true);

        if (drops.Count > 0)
        {
            itemText.text = "<align=\"center\"><u><size=70>Items Gained</size></u></align><size=45>\n\n";

            foreach (ScriptableItem iDrop in drops)
            {
                itemText.text += "<align=\"center\">" + iDrop.name + "</align>\n";
            }

            itemVisual.SetActive(true);
        }

        XPVisual.SetActive(false);
    }

    public void ActivateXPVisual()
    {
        winVisual.SetActive(false);
        itemVisual.SetActive(false);
        XPVisual.SetActive(true);
    }

    public void SetXPStats(string heroName, HeroStats beforeStats, HeroStats afterStats, List<ScriptableAttack> newAttacks = null)
    {
        playerName.text = heroName;
        levelAmount.text = "Level " + beforeStats.Level + " -> <color=#FFD300>" + afterStats.Level + "</color>";

        if (newAttacks != null)
        {
            newAttackVisual.SetActive(true);

            newAttacksText.text = "<align=\"center\"><u>New Attacks Unlocked</u></align>\n";

            foreach (ScriptableAttack attack in newAttacks)
            {
                newAttacksText.text += "<align=\"center\">" + attack.name + "</align>\n";
            }
        }
        else
        {
            newAttackVisual.SetActive(false);
        }

        healthAmount.text = beforeStats.Health + " -> <color=#FFD300>" + afterStats.Health + "</color>";
        defenseAmount.text = beforeStats.Defense + " -> <color=#FFD300>" + afterStats.Defense + "</color>";
        speedAmount.text = beforeStats.Speed + " -> <color=#FFD300>" + afterStats.Speed + "</color>";
        strengthAmount.text = beforeStats.Attack + " -> <color=#FFD300>" + afterStats.Attack + "</color>";
        staminaAmount.text = beforeStats.Stamina + " -> <color=#FFD300>" + afterStats.Stamina + "</color>";
    }
}
