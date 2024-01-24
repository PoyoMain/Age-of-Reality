using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Melee Attack")]
public class ScriptableMeleeAttack : ScriptableAttack
{
    void Awake()
    {
        _attackType = AttackType.Melee;
    }
}
