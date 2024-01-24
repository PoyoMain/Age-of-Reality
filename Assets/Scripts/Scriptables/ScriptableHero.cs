using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Hero")]
public class ScriptableHero : ScriptableUnitBase
{
    public HeroClass Class;

    public List<ScriptableMeleeAttack> meleeAttacks;
}

public enum HeroClass
{
    Warrior,
    Mage,
}

public enum HeroMoves
{
    Melee,
    Magic
}
