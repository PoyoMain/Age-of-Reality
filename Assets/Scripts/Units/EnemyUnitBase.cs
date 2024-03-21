using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnitBase : UnitBase
{
    [HideInInspector] public ScriptableEnemy data;

    public EnemyStats Stats { get; private set; }
    public int MaxHealth { get; private set; }

    /// <summary>
    /// Initialize the stats of the unit
    /// </summary>
    /// <param name="stats">The new stats</param>
    public virtual void InitStats(EnemyStats stats)
    {
        EnemyStats temp = stats;
        temp.Health = stats.Health * 100;
        MaxHealth = temp.Health;
        Stats = temp;
    }

    /// <summary>
    /// Set the stats of the unit
    /// </summary>
    /// <param name="stats">The new stats</param>
    public virtual void SetStats(EnemyStats stats)
    {
        EnemyStats temp = stats;
        temp.Health = stats.Health;
        Stats = temp;
    }

    private void OnEnable()
    {
        BattleManager.OnBeforeStateChanged += OnStateChanged;
    }

    private void OnDisable()
    {
        BattleManager.OnBeforeStateChanged -= OnStateChanged;
    }

    private void OnStateChanged(BattleState state)
    {
        if (state == BattleState.EnemyTurn)
        {
            _canMove = true;
        }
    }

    protected virtual void ExecuteMove()
    {
        _canMove = false;
    }

    /// <summary>
    /// Method for the unit to attack another unit
    /// </summary>
    /// <param name="attack">The attack being used</param>
    /// <param name="target">The target of the attack</param>
    public override void Attack(ScriptableAttack attack, UnitBase target, float multiplier = 1f, float accuracy = 100f)
    {
        HeroUnitBase enemyTarget = target as HeroUnitBase;

        StartCoroutine(AttackCoroutine(attack, enemyTarget));
    }

    IEnumerator AttackCoroutine(ScriptableAttack attack, HeroUnitBase target, float multiplier = 1f, float accuracy = 100f)
    {
        _anim.SetTrigger("Attacked");

        while (!HasAttacked)
        {
            yield return null;
        }

        int damage = Mathf.RoundToInt(((attack.Stats.attackPower + (Stats.Attack * 10) - (target.Stats.Defense * 10)) * multiplier) * (accuracy / 100));
        target.TakeDamage(damage);
        print("Enemy Damage Dealt: " + damage);

        yield break;
    }

    /// <summary>
    /// Damages the unit
    /// </summary>
    /// <param name="damage">The amount of damage to deal</param>
    public override void TakeDamage(int damage)
    {
        _anim.SetTrigger("Hit");
        EnemyStats newStats = Stats;
        newStats.Health -= damage;
        SetStats(newStats);
    }

    public override void SetEffects(ScriptableItem item)
    {
        switch (item.Effect)
        {
            case ItemEffect.Heal:
                Effect newEffect = new()
                {
                    durationInTurns = item.EffectDurationInTurns,
                    effect = item.Effect,
                    itemCausingEffect = item,
                };
                currentEffects.Add(newEffect);
                break;
        }
    }

    public override void DoEffects()
    {
        foreach (Effect effect in currentEffects)
        {
            switch (effect.effect)
            {
                case ItemEffect.Heal:
                    Heal(effect.itemCausingEffect);
                    break;
                case ItemEffect.AttackBoost:
                    break;
                case ItemEffect.Evasiveness:
                    break;
            }

            effect.durationInTurns--;

            if (effect.durationInTurns <= 0)
            {
                currentEffects.Remove(effect);
            }
        }
    }

    void Heal(ScriptableItem item)
    {
        TakeDamage(-item.EffectAmount);
    }

    public override void Select()
    {
        SendMessageUpwards("EnemySelected", this);
    }
}
