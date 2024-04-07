using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public abstract class UnitBase : MonoBehaviour
{
    protected Animator _anim;

    protected Outline outline;

    public bool IsSelected;
    public bool HasAttacked
    {
        get;
        protected set;
    }

    protected List<Effect> currentEffects = new();

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        outline = GetComponent<Outline>();

        HasAttacked = false;
    }

    /// <summary>
    /// Damages the unit
    /// </summary>
    /// <param name="damage">The amount of damage to deal</param>
    public abstract void TakeDamage(int damage);

    protected bool _canMove;

    /// <summary>
    /// Method for the unit to attack another unit
    /// </summary>
    /// <param name="attack">The attack being used</param>
    /// <param name="target">The target of the attack</param>
    //public abstract int Attack(ScriptableAttack attack, UnitBase target, float multiplier = 1f, float accuracy = 100f);

    /// <summary>
    /// Moves the unit
    /// </summary>
    /// <param name="newPos">The new position</param>
    /// <param name="speed">The speed the unit moves</param>
    /// <returns>Whether the unit has reached its destination</returns>
    public bool Move(Vector3 newPos, float speed)
    {
        newPos.Set(newPos.x, transform.position.y, newPos.z);
        Vector3 pos = transform.position;
        pos = Vector3.MoveTowards(pos, newPos, Time.deltaTime * speed);
        transform.position = pos;

        float x1 = transform.position.x;
        float y1 = transform.position.z;
        float x2 = newPos.x;
        float y2 = newPos.z;

        float distance = Mathf.Sqrt(Mathf.Pow(x2 - x1, 2) + Mathf.Pow(y2 - y1, 2));

        return distance <= 0.01f;
    }

    public void DecreaseEffects()
    {

    }

    public abstract void SetEffects(ScriptableItem item);

    public abstract void DoEffects();

    public abstract void Select();

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (Vector2.Distance(GameManager.Instance.GetBattleCamera().ScreenToWorldPoint(Input.mousePosition), transform.position) < 1f)
            {
                Select();
            }
        }

        if (!IsSelected)
        {
            outline.enabled = false;
        }
        else
        {
            outline.enabled = true;
        }
    }

    void AttackDone()
    {
        HasAttacked = true;
    }

    public void AttackStateReset()
    {
        HasAttacked = false;
    }
}

public class Effect
{
    public int durationInTurns;
    public ItemEffect effect;
    public ScriptableItem itemCausingEffect;
}