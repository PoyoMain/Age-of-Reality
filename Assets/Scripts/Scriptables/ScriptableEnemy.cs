using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Enemy")]
public class ScriptableEnemy : ScriptableUnitBase
{ 
    public EnemyClass Class;
}

public enum EnemyClass
{
    Slime,
    Orc,
    Wolf,
    Dragon,
    Looter
}