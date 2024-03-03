using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public enum Items { HealthPot, Sword, Card }
public class ScriptableInventory : ScriptableObject
{
    // Start is called before the first frame update
   public Dictionary<Items, Item> Inventory;

}
public class Item 
{
    ScriptableItem item;
    int Amount;
}
