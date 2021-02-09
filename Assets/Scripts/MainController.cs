using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
    public Transform wall;
    public Transform objpa;
    public Transform leftd;
    public Transform upr;
    public Transform poolpa;
    ZKobject zk;

    public float X;
    public float Y;
    public float speed = 0.1f;

    float oldp;
    private float updateinterval;

    Dictionary<Type, TrPool<Pobject>> pools;
    MainPanel mianp;

    private void Awake()
    {
        foreach (Transform item in wall)
        {
            item.GetComponent<SpriteRenderer>().enabled = false;
        }
        pools = new Dictionary<Type, TrPool<Pobject>>();
        var p = new TrPool<Pobject>(Resources.Load<Transform>("prefab/zk"), 50);
        p.repa = poolpa;
        pools.Add(typeof(ZKobject), p);
        p = new TrPool<Pobject>(Resources.Load<Transform>("prefab/fireboll"), 50);
        p.repa = poolpa;
        pools.Add(typeof(Ball), p);
        p = new TrPool<Pobject>(Resources.Load<Transform>("prefab/zd"), 5);
        p.repa = poolpa;
        pools.Add(typeof(ZDobject), p);
        //MVC.OnApplicationFocus += OnApplicationFocus;
    }

    void Start()
    {
        mianp = GameApp.ui.LoadPanel<MainPanel>(CanvasType.Main);
        X = leftd.position.x + 1.25f;
        Y = upr.position.y + 1.25f;
        Creat();

        var y2 = Y - 2.5f;
        for (int i = 0; i < 8; i++)
        {
            var l = X + i * 2.5f;
            var p = pools[typeof(ZKobject)].Getprefab(objpa);
            p.Item1.position = new Vector3(l, y2, 0);
        }

        MVC.IsBallSafeArea = IsBallDie;
        MVC.OnprefebDie = Reprefab;
        MVC.OnAddscoll = OnAddscoll;
    }

    private void OnDestroy()
    {
        if (GameApp.Ins)
        {
            GameApp.ui.RemovePanel<MainPanel>();
        }
    }

    private void OnAddscoll(int add)
    {
        mianp.SetV(mianp.value + add);
    }

    private bool IsBallDie(Vector3 pos)
    {
        return CanPlay(pos);
    }

    public void Reprefab<T>(T t) where T : ITrPoolItem
    {
        pools[t.GetType()].Reprefab(t);
    }

    private void Creat()
    {
        for (int i = 0; i < 8; i++)
        {
            int a = CreakType();
            if (a == 0)
            {
                var l = X + i * 2.5f;
                var p = pools[typeof(ZKobject)].Getprefab(objpa);
                p.Item1.position = new Vector3(l, Y, 0); p.Item2.isdie = false;
            }
            else if (a == 1)
            {
                var l = X + i * 2.5f;
                var p = pools[typeof(ZDobject)].Getprefab(objpa);
                p.Item1.position = new Vector3(l, Y, 0); p.Item2.isdie = false;
            }
        }
    }

    public int CreakType()
    {
        var r = UnityEngine.Random.Range(0, 1f);
        if (r > 0.98f)
        {
            return 1;
        }
        else if (r > 0.48f)
        {
            return 0;
        }
        else
        {
            return -1;
        }
    }

    private void FixedUpdate()
    {
        if (CashData.Pause)
        {
            return;
        }
        var p = objpa.position;
        p.y -= Time.fixedDeltaTime * speed;
        objpa.position = p;
        if (oldp - p.y > 2.5f)
        {
            oldp = (int)(p.y / 2.5f) * 2.5f;
            p.y = oldp;
            objpa.position = p;
            Creat();
        }
        else
        {
            objpa.position = p;
        }
    }

    Vector3 startpos;
    Vector3 uppos;

    Ball tempball;
    private void Update()
    {
        if (GameApp.ui.IsPointerOverGameObject() || CashData.Pause)
        {
            return;
        }
        foreach (var item in Input.touches)
        {
            if (GameApp.ui.IsPointerOverGameObject(item.fingerId))
            {
                return;
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
            var pos = Input.mousePosition;
            pos.z = -GameApp.camera.cameraTr.position.z;
            if (GameApp.camera.ScreenPointToWorld(ref pos))
            {
                if (objpa.childCount > 0)
                {
                    if (pos.y > objpa.GetChild(0).position.y - 2.5f)
                    {
                        return;
                    }
                }
                var c = CanPlay(pos);
                if (c)
                {
                    startpos = pos;
                }
                if (Physics2D.CircleCast(startpos, 0.5f, Vector2.zero))
                {

                }
                else
                {
                    var p = pools[typeof(Ball)].Getprefab();
                    p.Item1.position = startpos;
                    tempball = p.Item2 as Ball;
                    tempball.Beginsimulated();
                }
            }
        }
        else if (Input.GetMouseButton(0))
        {
            if (tempball.gameObject.activeSelf)
            {
                var pos = Input.mousePosition;
                pos.z = -GameApp.camera.cameraTr.position.z;
                if (GameApp.camera.ScreenPointToWorld(ref pos))
                {
                    var c = CanPlay(pos);
                    if (c)
                    {
                        if (Vector3.Distance(pos, startpos) < 3)
                        {
                            var x = Input.GetAxis("Mouse X");
                            var y = Input.GetAxis("Mouse Y");
#if UNITY_EDITOR
                            tempball.Fire(new Vector2(x, y) * 100 * Time.deltaTime);
#else 
                            tempball.Fire(new Vector2(x, y) * 40 * Time.deltaTime);
#endif
                        }
                    }
                }
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (tempball.gameObject.activeSelf)
            {
                if (tempball.Getvelocity().magnitude < 10)
                {
                    MVC.OnprefebDie(tempball);
                }
            }
        }

        updateinterval += Time.deltaTime;
        if (updateinterval > 1000)
        {
            updateinterval = 0;
        }
        if (objpa.GetChild(0).position.y < leftd.position.y)
        {
            CashData.Pause = true;
            MessageBox.Ins.ShowOk("", "游戏结束", "好的", (x) =>
            {
                CashData.Pause = false;
                GameApp.scene.Load("Main");
            });
        }
    }

    public bool CanPlay(Vector3 pos)
    {
        if (pos.x < leftd.position.x || pos.y < leftd.position.y || pos.x > upr.position.x || pos.y > upr.position.y)
        {
            return false;
        }
        return true;
    }
}
