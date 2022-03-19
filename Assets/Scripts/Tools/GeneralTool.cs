using System.Collections.Generic;
using UnityEngine;

public static class GeneralTool
{
    public static Color ResetAlpha(this Color color,float alpha) => new Color(color.r, color.g, color.b, alpha);

    public static void Log<T>(this List<T> list)
    {
        string s = null;
        foreach (T item in list)
        {
            s += item.ToString() + "|";
        }
        Debug.Log(s);
    }
}
