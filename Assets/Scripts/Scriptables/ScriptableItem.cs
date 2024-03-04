using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Item")]
public class ScriptableItem : ScriptableObject
{
    public string Description;
    public Sprite Sprite;
    public ItemEffect Effect;
    public float EffectAmount;
    public float EffectDuration;

}

public enum ItemEffect
{
    Heal,
    AttackBoost,
    Evasiveness
}