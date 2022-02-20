using UnityEngine;

public static class GeometryTool
{
    //此类中的角度的含义：朝上（0，1）为0°，顺时针为角度增大的方向（不超过360°）

    /// <summary>
    /// 角度转二维矢量
    /// </summary>
    public static Vector2 ToDirection(this float angle)
        => new Vector2(Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
    /// <summary>
    /// 二维矢量转角度
    /// </summary>
    public static float ToAngle(this Vector2 direction)
    {
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;
        return angle;
    }
    public static Vector3 ResetZ(this Vector3 v)
        => new Vector3(v.x, v.y, 0f);
    public static Vector3 AddZ(this Vector2 v)
        => new Vector3(v.x, v.y);
    public static Vector2 RemoveZ(this Vector3 v)
        => new Vector2(v.x, v.y);

    public static Vector2 ToVector2(this string s)
    {
        if (s == "") return Vector2.zero;
        string[] strs = s.Split(',');
        try
        {
            float x = float.Parse(strs[0]);
            float y = float.Parse(strs[1]);
            return new Vector2(x, y);
        }
        catch
        {
            Debug.LogWarning($"输入为“{s}”，格式不正确");
            return Vector2.zero;
        }
    }
    public static float ManhattanDistance(Vector3 a, Vector3 b, bool includingY = true)
        => Mathf.Abs(a.x - b.x) + (includingY ? Mathf.Abs(a.y - b.y) : 0f) + Mathf.Abs(a.z - b.z);
}

