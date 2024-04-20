using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class XPWindow : MonoBehaviour
{
    [Header("Win Popup Variables")]
    [SerializeField] private GameObject winVisual;
    [Space(15f)]
    [SerializeField] private ItemSlot itemSlotPrefab;
    [SerializeField] private Transform itemParent;
    [Space(15f)]
    [SerializeField] private TextMeshProUGUI xpGainText;

    [Space(20f)]
    [Header("XP Popup Variables")]
    [SerializeField] private GameObject XPVisual;
    [Space(15f)]
    [SerializeField] private TextMeshProUGUI playerName;
    [SerializeField] private TextMeshProUGUI levelAmount;
    [Space(15f)]
    [SerializeField] private TextMeshProUGUI healthAmount;
    [SerializeField] private TextMeshProUGUI defenseAmount;
    [SerializeField] private TextMeshProUGUI speedAmount;
    [SerializeField] private TextMeshProUGUI strengthAmount;
    [SerializeField] private TextMeshProUGUI staminaAmount;
    [Space(10f)]
    [SerializeField] private TextMeshProUGUI strengthText;
    [SerializeField] private TextMeshProUGUI staminaText;
    [Space(15f)]
    [SerializeField] private GameObject newAttackVisual;
    [SerializeField] private TextMeshProUGUI newAttacksText;

    
    //[SerializeField] private GameObject itemVisual;
    //[SerializeField] private TextMeshProUGUI itemText;

    private void Awake()
    {
        winVisual.SetActive(false);
        //itemVisual.SetActive(false);
        XPVisual.SetActive(false);
    }

    public void ActivateWinVisual(List<ScriptableItem> drops, int xpGained)
    {
        winVisual.SetActive(true);

        xpGainText.text = "You gained\n" + xpGained + " XP";

        foreach (Transform child in itemParent)
        {
            Destroy(child.gameObject);
        }

        foreach (ScriptableItem item in drops)
        {
            ItemSlot itemSlot = Instantiate(itemSlotPrefab, itemParent);
            itemSlot.SetItem(item.Sprite, item.name);
        }

        //if (drops.Count > 0)
        //{
        //    itemText.text = "<align=\"center\"><u><size=70>Items Gained</size></u></align><size=45>\n\n";

        //    foreach (ScriptableItem iDrop in drops)
        //    {
        //        itemText.text += "<align=\"center\">" + iDrop.name + "</align>\n";
        //    }

        //    itemVisual.SetActive(true);
        //}

        XPVisual.SetActive(false);
    }

    public void ActivateXPVisual()
    {
        winVisual.SetActive(false);
        //itemVisual.SetActive(false);
        XPVisual.SetActive(true);
    }

    public void SetXPStats(string heroName, bool magic, HeroStats beforeStats, HeroStats afterStats, List<ScriptableAttack> newAttacks = null)
    {
        playerName.text = heroName;
        levelAmount.text = "Level " + beforeStats.Level + "         <color=#FFD300>" + afterStats.Level + "</color>";

        if (newAttacks.Count > 0)
        {
            newAttackVisual.SetActive(true);

            newAttacksText.text = "";

            foreach (ScriptableAttack attack in newAttacks)
            {
                newAttacksText.text += "<align=\"center\">" + attack.name + "</align>\n";
            }
        }
        else
        {
            newAttackVisual.SetActive(false);
        }

        healthAmount.text = beforeStats.Health + "         <color=#FFD300>" + afterStats.Health + "</color>";
        defenseAmount.text = beforeStats.Defense + "         <color=#FFD300>" + afterStats.Defense + "</color>";
        speedAmount.text = beforeStats.Speed + "         <color=#FFD300>" + afterStats.Speed + "</color>";
        strengthAmount.text = beforeStats.Attack + "         <color=#FFD300>" + afterStats.Attack + "</color>";
        staminaAmount.text = beforeStats.Stamina + "         <color=#FFD300>" + afterStats.Stamina + "</color>";

        

        if (magic)
        {
            strengthText.text = "Intellegence";
            staminaText.text = "Wisdom";
        }
        else
        {
            strengthText.text = "Strength";
            staminaText.text = "Stamina";
        }
    }
}
