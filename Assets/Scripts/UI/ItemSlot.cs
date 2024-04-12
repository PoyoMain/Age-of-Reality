using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemName;

    public void SetItem(Sprite itemSprite, string itmName)
    {
        itemImage.sprite = itemSprite;
        itemName.text = itmName;
    }
}
