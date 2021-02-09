using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : Pobject
{

    public int atk = 1;
    Rigidbody2D rig;

    protected override void Awake()
    {
        base.Awake();
        rig = Tr.GetComponent<Rigidbody2D>();
        rig.simulated = false;
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        var res = MVC.IsBallSafeArea.Invoke(Tr.position);
        if (!res)
        {
            isdie = true;
            MVC.OnprefebDie?.Invoke(this);
        }
        else
        {
            var zk = collision.transform.GetComponent<Pobject>();
            if (zk)
            {
                zk.OnHit(atk);
            }
        }
    }

    public override void OnEnterPool()
    {
        base.OnEnterPool();
        rig.simulated = false;
    }

    public void Beginsimulated()
    {
        rig.simulated = true;
    }

    public void Fire(Vector2 force)
    {
        rig.AddForce(force,ForceMode2D.Impulse);
    }

    public Vector2 Getvelocity()
    {
        return rig.velocity;
    }

    protected override void OnPause(bool pause)
    {
        base.OnPause(pause);
        rig.simulated = !pause;
    }

    public override void Message(string mes)
    {

    }
}
