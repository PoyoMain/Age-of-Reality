using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyUnitBase : UnitBase
{
    public GameObject selectIndicator;

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
}
