using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZDobject : Pobject
{
    public GameObject boom;
    public override void Start()
    {
        base.Start();
        var sp = Resources.Load<Sprite>("sprite/zd");
        spriteRenderer.sprite = sp;
    }

    public override void OnHit(int h)
    {
        if (isdie)
            return;
        heart -= h;
        if (heart <= 0)
        {
            GameApp.audio.PlayOneShot(AppAudio.AudioPath.eff, CashData.boom);
            isdie = true;
            var b = Instantiate(boom, Tr.position, Quaternion.identity);
            Destroy(b, 3f);
            MVC.OnAddscoll?.Invoke(2);

            var trs = Physics2D.CircleCastAll(Tr.position, 2, Vector2.zero);
            Debug.Log(trs.Length);
            foreach (var item in trs)
            {
                var p = item.transform.GetComponent<Pobject>();
                if (p)
                {
                    p.Message(CashData.boom);
                }
            }
            MVC.OnprefebDie?.Invoke(this);
        }
        else
        {
            GameApp.audio.PlayOneShot(AppAudio.AudioPath.eff, CashData.pao);
        }
    }

    public override void Message(string mes)
    {
        if (mes == CashData.boom)
        {
            if (!isdie)
            {
                isdie = true;
                MVC.OnprefebDie?.Invoke(this);
            }
        }
    }

}
