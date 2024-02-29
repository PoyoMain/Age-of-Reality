using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class PlayerInventory : ScriptableObject
{
    // Start is called before the first frame update
    enum Items { HealthPot, Sword, Card }
    Dictionary<Items, Item> Inventory;
    
}
public class Item : MonoBehaviour
{
    ScriptableItem item;
    int Amount;
}
