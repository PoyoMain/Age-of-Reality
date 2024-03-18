using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private List<ScriptableEnemy> allies;
    [HideInInspector] public List<ScriptableEnemy> team; // The enemy party including this enemy
    public ScriptableEnemy type;

    private readonly float invincibleTime = 5f; // Time enemy is invincible
    private float invincibleTimer; // Timer checking how long enemy has been invincible for

    private SpriteRenderer sRend;
    private Collider2D coll;
    private Rigidbody2D rigid;

    public bool isBoss;

    [TextArea(2,2)]
    [SerializeField] private string[] lines;
    public string[] Lines 
    { 
        get { return lines; }
    }

    // Start is called before the first frame update
    void Start()
    {
        team.Add(type);
        team.AddRange(allies);

        sRend = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
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
            sRend.enabled = !sRend.enabled;
            yield return null;
        }

        sRend.enabled = true;
        coll.enabled = true;

        yield return null;
    }

    public void Freeze()
    {
        rigid.velocity = Vector3.zero;
    }
    
}
