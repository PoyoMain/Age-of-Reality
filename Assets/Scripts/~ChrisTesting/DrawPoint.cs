using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
public class DrawPoint : MonoBehaviour
{
    [HideInInspector] public bool hit;
    private SpriteRenderer sRend;

    private void Awake()
    {
        sRend = GetComponent<SpriteRenderer>();
        sRend.enabled = false;
    }

    public bool Active
    {
        get;
        private set;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && Active)
        {
            Deactivate();
        }
    }

    public void Activate()
    {
        Active = true;
        sRend.enabled = true;
    }

    public void Deactivate()
    {
        Active = false;
        sRend.enabled = false;
    }
}
