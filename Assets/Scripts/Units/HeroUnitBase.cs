using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroUnitBase : UnitBase
{
    [HideInInspector] public ScriptableHero data;
    private readonly Dictionary<ItemEffect, int> effectTurns = new()
    {
        { ItemEffect.Heal, 0 },
        { ItemEffect.AttackBoost, 0 },
        { ItemEffect.Evasiveness, 0 },
    };

    public HeroStats Stats { get; private set; }
    public int MaxHealth {  get; private set; }

    /// <summary>
    /// Initialize the stats of the unit
    /// </summary>
    /// <param name="stats">The new stats</param>
    public virtual void InitStats(HeroStats stats)
    {
        HeroStats temp = stats;
        temp.Health = 100 + ((stats.Health * 10) - 10);
        MaxHealth = temp.Health;
        Stats = temp;
        
        if (data.Ability == Ability.Magic) _anim.SetBool("Magic", true);
        else _anim.SetBool("Magic", false);
    }

    /// <summary>
    /// Set the stats of the unit
    /// </summary>
    /// <param name="stats">The new stats</param>
    public virtual void SetStats(HeroStats stats)
    {
        HeroStats temp = stats;
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
    public int Attack(ScriptableAttack attack, UnitBase target, float accuracy = 100f)
    {
        EnemyUnitBase enemyTarget = target as EnemyUnitBase;
        //int damage = Mathf.RoundToInt(((attack.Stats.attackPower + (Stats.Attack * 10) - (enemyTarget.Stats.Defense * 10)) * multiplier) * (accuracy / 100));
        int damage = Mathf.RoundToInt((((attack.Stats.attackPower * ((1 + ((Stats.Attack - 1) / 10)))) - (enemyTarget.Stats.Defense - 1)) * attack.Stats.multiplier) * (accuracy / 100));

        

        StartCoroutine(AttackCoroutine(enemyTarget, damage, attack is ScriptableMagicAttack, attack.Stats.multiplier));
        
        return damage;
    }

    IEnumerator AttackCoroutine(EnemyUnitBase target, int damage, bool magicAttack, int attackNum)
    { 
        _anim.SetTrigger("Attacked");

        while (!AttackFinished)
        {
            yield return null;
        }

        target.TakeDamage(damage, magicAttack, attackNum);
        print("Player Damage Dealt: " + damage);

        yield return new WaitForSeconds(0.5f);

        yield break;
    }

    /// <summary>
    /// Damages the unit
    /// </summary>
    /// <param name="damage">The amount of damage to deal</param>
    public void TakeDamage(int damage)
    {
        HeroStats newStats = Stats;
        newStats.Health -= damage;
        SetStats(newStats);
    }

    public void TakeDamage(int damage, bool magicAttack, int attackNum)
    {
        StartCoroutine(TakeDamageCoroutine(damage, magicAttack, attackNum));
    }

    private IEnumerator TakeDamageCoroutine(int damage, bool magicAttack, int attackNum)
    {
        PlayEffect(attackNum, magicAttack);

        while (!EffectFinished)
        {
            yield return null;
        }

        SendMessageUpwards("ShakeCamera");
        _anim.SetTrigger("Hit");

        HeroStats newStats = Stats;
        newStats.Health -= damage;
        SetStats(newStats);

        while (!FullAttackDone)
        {
            yield return null;
        }

        yield break;
    }

    public override void SetEffects(ScriptableItem item)
    {
        switch (item.Effect)
        {
            case ItemEffect.Heal:
                Heal(item);
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
        List<Effect> finishedEffects = new ();
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
                finishedEffects.Add(effect);
            }
        }

        foreach (Effect effect in  finishedEffects)
        {
            currentEffects.Remove(effect);
        }

        finishedEffects.Clear();
    }

    void Heal(ScriptableItem item)
    {
        HeroStats newStats = Stats;
        newStats.Health += item.EffectAmount;
        newStats.Health = Mathf.Clamp(newStats.Health, 0, data.BaseStats.Health);
        InitStats(newStats);
    }

    public override void Select()
    {
        SendMessageUpwards("PlayerSelected", this);
    }

    void PlayVocalAttackAudio()
    {
        AudioClip clipToPlay = data.vocalAttackSFX[Random.Range(0, data.vocalAttackSFX.Length)];
        AudioManager.Instance.PlayBattleSFX(clipToPlay);
    }

    void PlaySwordSwingAudio()
    {
        AudioClip clipToPlay = data.swordSwingSFX[Random.Range(0, data.swordSwingSFX.Length)];
        AudioManager.Instance.PlayBattleSFX(clipToPlay);
    }

    void PlayFireAudio()
    {
        AudioClip clipToPlay = data.fireShootSFX[Random.Range(0, data.fireShootSFX.Length)];
        AudioManager.Instance.PlayBattleSFX(clipToPlay);
    }

    void PlayHurtAudio()
    {
        AudioClip clipToPlay = data.hurtSFX[Random.Range(0, data.hurtSFX.Length)];
        AudioManager.Instance.PlayBattleSFX(clipToPlay);
    }
}