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
    public HeroStats BaseStats { get { return _stats; } private set { } }

    public List<ScriptableMeleeAttack> meleeAttacks;

    public List<ScriptableMagicAttack> magicAttacks;

    public void IncreaseLevel(int excessXP)
    {
        _stats.Level++;
        _stats.XP = excessXP;
        Debug.Log("Level Up");
    }

    public void ResetCharacter()
    {
        _stats.Level = 1;
        _stats.XP = 0;
    }

    public void IncreaseXP(int xp)
    {
        _stats.XP += xp;

        Debug.Log("XP " + _stats.XP);

        if (_stats.XP >= _stats.XPToLevelUp)
        {
            IncreaseLevel(_stats.XP - _stats.XPToLevelUp);

            if (_stats.XP >= _stats.XPToLevelUp)
            {
                IncreaseXP(0);
            }
        }
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
    public XPLevels XPLevels;

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

    public int XPToLevelUp
    {
        get
        {
            return GetXPToLevelUP();
        }
    }

    int GetXPToLevelUP()
    {
        int levelXP;

        switch (Level)
        {
            case 1:
                levelXP = XPLevels.Level1XP;
                break;

            case 2:
                levelXP = XPLevels.Level1XP;
                break;

            case 3:
                levelXP = XPLevels.Level1XP;
                break;

            case 4:
                levelXP = XPLevels.Level1XP;
                break;

            case 5:
                levelXP = XPLevels.Level1XP;
                break;

            default:
                levelXP = 0;
                break;
        }

        return levelXP;
    }
}

[Serializable]
public struct XPLevels
{
    public int Level1XP;
    public int Level2XP;
    public int Level3XP;
    public int Level4XP;
    public int Level5XP;
}