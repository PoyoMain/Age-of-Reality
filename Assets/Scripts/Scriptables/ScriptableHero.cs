using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Hero", menuName = "Scripatble Character/Hero")]
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
    public HeroStats BaseStats { get { return _stats; } private set { } }

    [Space(15)]
    public List<ScriptableMeleeAttack> meleeAttacks;

    public List<ScriptableMagicAttack> magicAttacks;

    [Space(15)]
    public AudioClip[] vocalAttackSFX;
    public AudioClip[] swordSwingSFX;
    public AudioClip[] fireShootSFX;
    public AudioClip[] hurtSFX;


    public void ResetCharacter()
    {
        _stats.Level = 1;
        _stats.XP = 0;
        _stats.Attack = 10;
        _stats.Health = 10;
        _stats.Defense = 10;
        _stats.Speed = 10;
        _stats.Stamina = 10;
        _stats.ExtraStatPoints = 0;
        meleeAttacks.RemoveRange(1, meleeAttacks.Count - 1);
        magicAttacks.RemoveRange(1, magicAttacks.Count - 1);
    }

    public void IncreaseXP(int xp)
    {
        _stats.XP += xp;

        Debug.Log("XP Gained: " + _stats.XP);
    }

    public bool LevelUp(ScriptableLevelSystem levelSystem, out ScriptableAttack unlockedAttack)
    {
        unlockedAttack = null;
        if (_stats.Level >= levelSystem.levels[^1].lvl) return false;

        Level nextLevel = null;

        foreach (Level level in levelSystem.levels)
        {
            if ((_stats.Level + 1) == level.lvl)
            {
                nextLevel = level;
                _stats.ExtraStatPoints += 2;
                break;
            }
        }

        unlockedAttack = null;

        if (_stats.XP >= nextLevel.XPToReachLevel)
        {
            IncreaseLevel(_stats.XP - nextLevel.XPToReachLevel);

            if (Ability == Ability.Melee)
            {
                ScriptableMeleeAttack newAttack = nextLevel.MeleeAttackUnlocked;
                if (newAttack != null)
                {
                    unlockedAttack = newAttack;
                    meleeAttacks.Add(newAttack);
                }
            }
            else if (Ability == Ability.Magic)
            {
                ScriptableMagicAttack newAttack = nextLevel.MagicAttackUnlocked;
                if (newAttack != null)
                {
                    unlockedAttack = newAttack;
                    magicAttacks.Add(newAttack);
                }
            }

            return true;
        }

        return false;

        //if (_stats.XP >= _stats.XPToLevelUp)
        //{
        //    IncreaseLevel(_stats.XP - _stats.XPToLevelUp);
        //    return true;
        //}

        //return false;
    }

    private void IncreaseLevel(int excessXP)
    {
        _stats.Level++;
        _stats.XP = excessXP;
        Debug.Log("Level Up");
    }

    public void IncreaseStat(StatType type)
    {
        switch (type)
        {
            case StatType.Health:
                _stats.Health++;
                break;
            case StatType.Attack:
                _stats.Attack++;
                break;
            case StatType.Defense:
                _stats.Defense++;
                break;
            case StatType.Speed:
                _stats.Speed++;
                break;
            case StatType.Stamina:
                _stats.Stamina++;
                break;
            case StatType.ExtraStatPoints:
                _stats.ExtraStatPoints++;
                break;
        }
    }

    public void DecreaseStatPoint()
    {
        _stats.ExtraStatPoints--;
    }

    private void OnEnable()
    {
        ResetCharacter();
    }

    private void OnDisable()
    {
        ResetCharacter();
    }

    private void OnDestroy()
    {
        ResetCharacter();
    }
}

[Serializable]
public struct HeroStats
{
    public int Health;
    public int Attack;
    public int Defense;
    public int Speed;
    public int Stamina;
    public int ExtraStatPoints;

    public int Level
    {
        get;
        set;
    }
    public int XP
    {
        get;
        set;
    }

    public void SetHealth(int newHealth)
    {
        Health = newHealth;
    }
}

public enum StatType
{
    Health,
    Attack,
    Defense,
    Speed,
    Stamina,
    ExtraStatPoints
}