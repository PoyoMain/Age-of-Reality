using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class HeroUnitBase : UnitBase
{
    [HideInInspector] public ScriptableHero data;

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
}
