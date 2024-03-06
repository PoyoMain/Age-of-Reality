using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Inventory")]
public class ScriptableInventory : ScriptableObject
{
    public List<DictEntry> Inventory;

    public bool IsEmpty
    {
        get
        {
            return IsInventoryEmpty();
        }
        private set { }
    }

    public void UseItem(ScriptableItem item)
    {
        foreach (DictEntry entry in Inventory)
        {
            if (entry.Value.item == item)
            {
                if (entry.Value.Amount > 0)
                {
                    entry.Value.Amount--;
                }
            }
        }
    }

    public bool HasItem(ScriptableItem itemToCheck)
    {
        foreach (DictEntry entry in Inventory)
        {
            if (entry.Value.item.Equals(itemToCheck))
            {
                if (entry.Value.Amount > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
        }

        return false;
    }

    private int ContainsItemEntry(ScriptableItem itemToCheck)
    {
        for (int i = 0; i < Inventory.Count; i++)
        {
            if (Inventory[i].Value.item.Equals(itemToCheck))
            {
                return i;
            }
        }

        return -1;
    }

    bool IsInventoryEmpty()
    {
        foreach (DictEntry item in Inventory)
        {
            if (item.Value.Amount > 0)
            {
                return false;
            }
        }

        return true;
    }

    public void AddToInventory(ScriptableItem itemToAdd)
    {
        int itemPos = ContainsItemEntry(itemToAdd);
        if (itemPos != -1)
        {
            Inventory[itemPos].Value.Amount++;
        }
        else
        {
            Items newType = itemToAdd.Type;
            Inventory.Add(new DictEntry(newType, new Item(itemToAdd)));
        }
    }

    private void OnDisable()
    {
        Inventory.Clear();
    }
}
public enum Items { HealthPot, Sword, Card }

[Serializable]
public class Item 
{
    public ScriptableItem item;
    public int Amount;

    public Item(ScriptableItem itemToAdd) 
    {
        item = itemToAdd;
        Amount = 1;
    }
}

[Serializable]
public class DictEntry
{
    public Items Key;
    public Item Value;

    public DictEntry(Items newKey, Item newValue)
    {
        this.Key = newKey;
        this.Value = newValue;
    }
}

