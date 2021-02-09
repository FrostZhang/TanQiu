using UnityEngine;
using System;

public partial class MVC
{
    //public static System.Action TempAction;
    public static Func<Vector3, bool> IsBallSafeArea;
    public static Action<ITrPoolItem> OnprefebDie;
    public static Action<bool> OnPause;

    public static Action<int> OnAddscoll;
}
