using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Hero")]
public class ScriptableHero : ScriptableUnitBase
{
    /// <summary>
    /// Party members
    /// </summary>
    public Ability Ability;

    /// <summary>
    /// Stats of this unit
    /// </summary>
    [SerializeField] private HeroStats _stats;
    public HeroStats BaseStats { get { return _stats; } }

    public List<ScriptableMeleeAttack> meleeAttacks;

    public List<ScriptableMagicAttack> magicAttacks;
}

[Serializable]
public struct HeroStats
{
    public int Health;
    public int Attack;
    public int Defense;
    public int Speed;
    public int Stamina;
}