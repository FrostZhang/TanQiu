using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZKobject : Pobject
{
    public static Sprite[] sps;

    public override void Start()
    {
        base.Start();
        if (sps == null)
        {
            sps = Resources.LoadAll<Sprite>("sprite/zk");
        }
        int i = UnityEngine.Random.Range(0, sps.Length);
        spriteRenderer.sprite = sps[i];
    }

    public override void OnHit(int h)
    {
        if (isdie)
            return;
        GameApp.audio.PlayOneShot(AppAudio.AudioPath.eff, CashData.pao);
        heart -= h;
        if (heart <= 0)
        {
            isdie = true;
            MVC.OnAddscoll?.Invoke(2);
            MVC.OnprefebDie?.Invoke(this);
        }
    }

    public override void Message(string mes)
    {
        if (mes == CashData.boom)
        {
            if (!isdie)
            {
                isdie = true;
                MVC.OnAddscoll?.Invoke(2);
                MVC.OnprefebDie?.Invoke(this);
            }
        }
    }
}
