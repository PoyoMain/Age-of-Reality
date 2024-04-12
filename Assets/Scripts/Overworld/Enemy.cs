using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

[RequireComponent(typeof(Animator))]
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
    private Animator anim;
    private Chase chaseScript;
    private Patrol patrolScript;

    public bool isBoss;

    [SerializeField] private DialogueLine line;

    public DialogueLine Line
    {
        get { return line; }
    }


    void Awake()
    {
        team.Add(type);
        team.AddRange(allies);

        sRend = GetComponent<SpriteRenderer>();
        coll = GetComponent<Collider2D>();
        rigid = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        chaseScript = GetComponent<Chase>();
        patrolScript = GetComponent<Patrol>();
    }

    private void Start()
    {
        anim.SetFloat("X", 0);
        anim.SetFloat("Y", -1);
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
        anim.speed = 0;
        rigid.velocity = Vector3.zero;

        if (chaseScript != null) chaseScript.enabled = false;
        if (patrolScript != null) patrolScript.enabled = false;
    }

    public void UnFreeze()
    {
        anim.speed = 1;

        if (chaseScript != null) chaseScript.enabled = true;
        if (patrolScript != null) patrolScript.enabled = true;
    }
    
}

[Serializable]
public class DialogueLine
{
    [TextArea(2, 2)]
    public string Line;
    public float textSpeed;

    [Space(10)]
    public AudioClip[] VoiceClips;
}
