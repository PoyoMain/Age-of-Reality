using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Melee Attack", menuName = "Scripatble Attack/Melee")]
public class ScriptableMeleeAttack : ScriptableAttack
{
    void Awake()
    {
        _attackType = AttackType.Melee;
    }
}
