using UnityEngine;

public partial class CashData
{
    private static bool pause;

    public static bool Pause
    {
        get
        {
            return pause;
        }

        set
        {
            MVC.OnPause?.Invoke(value);
            pause = value;
        }
    }

    public const string boom = "boom";
    public const string pao = "pao";
}
