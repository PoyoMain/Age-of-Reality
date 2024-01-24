using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class AttackOption : MoveOption
{
    public ScriptableAttack Attack; // The attack corresponding to this option
}
