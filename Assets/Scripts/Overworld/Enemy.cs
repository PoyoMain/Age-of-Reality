using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private List<ScriptableEnemy> allies;
    [HideInInspector] public List<ScriptableEnemy> team; // The enemy party including this enemy
    public EnemyClass type;

    private readonly float invincibleTime = 5f; // Time enemy is invincible
    private float invincibleTimer; // Timer checking how long enemy has been invincible for

    private MeshRenderer mRend;
    private Collider coll;
    private Rigidbody2D rigid;

    // Start is called before the first frame update
    void Start()
    {
        team.Add(ResourceStorage.Instance.GetEnemy(Enum.GetName(typeof(EnemyClass), type)));
        team.AddRange(allies);

        mRend = GetComponent<MeshRenderer>();
        coll = GetComponent<Collider>();
        rigid = GetComponent<Rigidbody2D>();
    }


    /// <summary>
    /// Turns the enemy invisible for a certain amount of time
    /// </summary>
    public void MakeInvincible()
    {
        invincibleTimer = Time.time + invincibleTime; 

        StartCoroutine(Invincibility());
    }

    IEnumerator Invincibility()
    {
        coll.enabled = false;

        while (invincibleTimer > Time.time)
        {
            mRend.enabled = !mRend.enabled;
            yield return null;
        }

        mRend.enabled = true;
        coll.enabled = true;

        yield return null;
    }

    public void Freeze()
    {
        rigid.velocity = Vector3.zero;
    }
    
}
