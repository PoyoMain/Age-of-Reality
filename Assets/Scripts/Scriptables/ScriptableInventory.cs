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

