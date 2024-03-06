using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Magic Attack", menuName = "Scripatble Attack/Magic")]
public class ScriptableMagicAttack : ScriptableAttack
{
    void Awake()
    {
        _attackType = AttackType.Magic;
    }
}
