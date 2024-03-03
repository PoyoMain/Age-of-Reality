using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableItem : ScriptableObject
{
    public string Name;
    public string Description;
    public Sprite Sprite;
    public ItemEffect Effect;

}

public enum ItemEffect
{
    Heal,
    AttackBoost,
    Evasiveness
}