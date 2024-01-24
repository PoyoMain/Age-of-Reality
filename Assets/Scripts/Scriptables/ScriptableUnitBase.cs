using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableUnitBase : ScriptableObject
{
    /// <summary>
    /// Party members
    /// </summary>
    public Party Party;

    /// <summary>
    /// Stats of this unit
    /// </summary>
    [SerializeField] private Stats _stats;
    public Stats BaseStats { get { return _stats; } }

    /// <summary>
    /// Prefab of this unit to spawn in
    /// </summary>
    public UnitBase Prefab;

    /// <summary>
    /// Description of unit
    /// </summary>
    public string Description;

    /// <summary>
    /// Unit's sprite
    /// </summary>
    public Sprite Sprite;
}

[Serializable]
public struct Stats
{
    public int Health;
    public int Attack;
    public int Defense;
    public int Speed;
}

public enum Party
{
    Hero,
    Enemy
}
