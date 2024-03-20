using System;
using System.Collections;
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

    public List<ScriptableMeleeAttack> meleeAttacks;

    public List<ScriptableMagicAttack> magicAttacks;

    public void ResetCharacter()
    {
        _stats.Level = 1;
        _stats.XP = 0;
        _stats.Attack = 1;
        _stats.Health = 1;
        _stats.Defense = 1;
        _stats.Speed = 1;
        _stats.Stamina = 1;
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

    private void OnDisable()
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