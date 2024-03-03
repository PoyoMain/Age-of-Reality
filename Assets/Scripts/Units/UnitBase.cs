using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitBase : MonoBehaviour
{


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
    public abstract void Attack(ScriptableAttack attack, UnitBase target, float multiplier = 1f, float accuracy = 1f);

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
}
