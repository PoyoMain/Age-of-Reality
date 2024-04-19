using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableAttack : ScriptableObject
{
    /// <summary>
    /// The stats of the attack
    /// </summary>
    [SerializeField] private AttackStats _stats;
    public AttackStats Stats { get { return _stats; } }

    /// <summary>
    /// The type of attack this is
    /// </summary>
    protected AttackType _attackType;

    public LineMinigameBase[] Minigames;

    public int SecondsToComplete;
}

[Serializable]
public struct AttackStats
{
    public int attackPower;
    public int attackNumber;
    public float multiplier;
    public int AP;
}

public enum AttackType
{
    Melee,
    Magic
}

public enum MinigameType
{
    Magic_Triangle,
    Magic_Square,
    Magic_Pentagon,

    Melee_One,
    Melee_Two,
    Melee_Three,
}

