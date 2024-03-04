using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Inventory")]
public class ScriptableInventory : ScriptableObject
{
    [SerializeField]
    public List<DictEntry> Inventory;

    public bool IsEmpty
    {
        get
        {
            return isEmpty();
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

    public bool HasItem(Items key)
    {
        foreach (DictEntry entry in Inventory)
        {
            if (entry.Key.Equals(key))
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

    bool isEmpty()
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
    
}
public enum Items { HealthPot, Sword, Card }

[Serializable]
public class Item 
{
    public ScriptableItem item;
    public int Amount;
}

[Serializable]
public class DictEntry
{
    public Items Key;
    public Item Value;
}

