using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Enemy", menuName = "Scripatble Character/Enemy")]
public class ScriptableEnemy : ScriptableUnitBase
{
    /// <summary>
    /// Stats of this unit
    /// </summary>
    [SerializeField] private EnemyStats _stats;
    public EnemyStats BaseStats { get { return _stats; } }

    public List<ScriptableAttack> attacks;

    public List<ItemDrop> droppableItems;
}

[Serializable]
public struct EnemyStats
{
    public int Health;
    public int Attack;
    public int Defense;
    public int Speed;
    public int XP;
}

[Serializable]
public class ItemDrop
{
    public ScriptableItem Item;

    [Range(0f, 1f)] 
    public float ChanceToDrop;
}