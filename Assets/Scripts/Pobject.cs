using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pobject : MonoBehaviour, ITrPoolItem
{
    public bool isdie;
    public int heart;
    protected Collider2D cuscolleder;
    public Transform Tr { get; set; }

    protected SpriteRenderer spriteRenderer;
    protected virtual void Awake()
    {
        Tr = transform;
        cuscolleder = Tr.GetComponent<Collider2D>();
        spriteRenderer = Tr.GetComponent<SpriteRenderer>();
        MVC.OnPause += OnPause;
    }

    public virtual void OnHit(int h)
    {

    }

    protected virtual void OnDestroy()
    {
        MVC.OnPause -= OnPause;
    }

    protected virtual void OnPause(bool pause)
    {

    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {

    }

    public virtual void Message(string mes)
    {

    }

    public virtual void Start()
    {

    }

    public virtual void OnEnterPool()
    {
        heart = 0;
        isdie = false;
    }
}
