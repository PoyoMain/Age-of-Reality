using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Item", menuName = "Item")]
public class ScriptableItem : ScriptableObject
{
    public string Description;
    public Sprite Sprite;
    public ItemEffect Effect;
    public Items Type
    {
        get 
        {
            return GetItemType();
        }
        private set { }
    }
    public int EffectAmount;
    public int EffectDurationInTurns;

    Items GetItemType()
    {
        Items typeReturn = Effect switch
        {
            ItemEffect.Heal => Items.HealthPot,
            ItemEffect.AP => Items.MP,
            _ => Items.HealthPot,
        };
        return typeReturn;
    }
}

public enum ItemEffect
{
    Heal,
    AP,
    AttackBoost,
    Evasiveness
}