using System.Collections.Generic;
using UnityEngine;

public static class GeneralTool
{
    /// <summary>
    /// 获取一个颜色变得完全透明后的颜色
    /// </summary>
    public static Color TransparentColor(this Color color) => new Color(color.r, color.g, color.b, 0f);
    /// <summary>
    /// 获取一个颜色变得完全不透明后的颜色
    /// </summary>
    public static Color OpaqueColor(this Color color) => new Color(color.r, color.g, color.b, 1f);
    public static Color LightColor(this Color color) => new Color(color.r, color.g, color.b, 0.5f);
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
