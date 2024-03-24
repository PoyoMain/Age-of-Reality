using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ItemSelectMenu : MonoBehaviour
{
    [SerializeField] private GameObject itemButton;

    [HideInInspector] public HeroUnitBase currentUnit;

    private void OnEnable()
    {
        DeleteButtons();

        SpawnButtons();
    }

    void DeleteButtons()
    {
        foreach (Transform child in transform)
        {
            child.GetComponent<MoveOption>().button.onClick.RemoveAllListeners();
            Destroy(child.gameObject);
        }
    }

    void SpawnButtons()
    {
        if (itemButton == null)
        {
            Debug.LogError("Item Button prefab not set in inspector");
            return;
        }

        foreach (var item in GameManager.Instance.ItemInventory.Inventory)
        {

            if (GameManager.Instance.ItemInventory.HasItem(item.Value.item))
            {
                ItemOption moveOption = Instantiate(itemButton, transform).GetComponent<ItemOption>();
                moveOption.item = item.Value.item;
                moveOption.gameObject.name = item.Value.item.name;
                moveOption.GetComponentInChildren<TextMeshProUGUI>().text = item.Value.item.name + " x" + item.Value.Amount;
            }
        }
    }
    private void OnDisable()
    {
        DeleteButtons();
    }
}
