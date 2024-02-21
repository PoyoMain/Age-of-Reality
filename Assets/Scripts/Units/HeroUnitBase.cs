using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroUnitBase : UnitBase
{
    [HideInInspector] public ScriptableHero data;

    public HeroStats Stats { get; private set; }

    /// <summary>
    /// Sets the stats of the unit
    /// </summary>
    /// <param name="stats">The new stats</param>
    public virtual void SetStats(HeroStats stats)
    {
        Stats = stats;
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
        if (state == BattleState.HeroTurn)
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
    public override void Attack(ScriptableAttack attack, UnitBase target, float multiplier = 1f, float accuracy = 1f)
    {
        EnemyUnitBase enemyTarget = target as EnemyUnitBase;

        int damage = Mathf.RoundToInt(((attack.Stats.attackPower + (Stats.Attack * 10) - (enemyTarget.Stats.Defense * 10)) * multiplier) * accuracy);
        target.TakeDamage(damage);

        print(damage);
    }

    /// <summary>
    /// Damages the unit
    /// </summary>
    /// <param name="damage">The amount of damage to deal</param>
    public override void TakeDamage(int damage)
    {
        HeroStats newStats = Stats;
        newStats.Health -= damage;
        SetStats(newStats);
    }
}
