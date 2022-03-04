using UnityEngine;

public static class MathTool
{
    //此类中的角度的含义：朝上（0，1）为0°，逆针为角度增大的方向（不超过360°），这和欧拉角的定义一致

    /// <summary>
    /// 角度转二维矢量
    /// </summary>
    public static Vector2 ToDirection(this float angle)
        => new Vector2(-Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad)); 
    /// <summary>
    /// 二维矢量转角度
    /// </summary>
    public static float ToAngle(this Vector2 direction)
    {
        float angle = 360f - Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
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
        if (s == "") 
            return Vector2.zero;
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

    /// <summary>
    /// 计算曼哈顿距离
    /// </summary>
    /// <param name="includingY">是否包含y方向的距离</param>
    /// <returns></returns>
    public static float ManhattanDistance(Vector3 a, Vector3 b, bool includingY = true)
        => Mathf.Abs(a.x - b.x) + (includingY ? Mathf.Abs(a.y - b.y) : 0f) + Mathf.Abs(a.z - b.z);

    //判断point是否在从start到end的线段的左侧（投影到XY平面）
    public static bool OnLeft(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 line1 = (end - start).ResetZ();
        Vector3 line2 = (point - start).ResetZ();
        return Vector3.Cross(line2, line1).z >= 0;
    }
    //判断起点相同时，向量line1是否在line2的左侧（投影到XY平面）
    public static bool OnLeft(Vector3 line1, Vector3 line2)
    {
        line1 = line1.ResetZ();
        line2 = line2.ResetZ();
        return Vector3.Cross(line2, line1).z >= 0;
    }
    public static bool OnRight(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 line1 = (end - start).ResetZ();
        Vector3 line2 = (point - start).ResetZ();
        return Vector3.Cross(line2, line1).z <= 0;
    }
    public static bool OnRight(Vector3 line1, Vector3 line2)
    {
        line1 = line1.ResetZ();
        line2 = line2.ResetZ();
        return Vector3.Cross(line2, line1).z <= 0;
    }
    /// <summary>
    /// 计算重心坐标
    /// </summary>
    /// <returns>三个分量分别表示A、B、C的权重</returns>
    public static Vector3 BarycentricCoordinates(Vector3 A,Vector3 B,Vector3 C,Vector3 P)
    {
        float alpha, beta, gamma;
        float x, xa, xb, xc, y, ya, yb, yc;
        x = P.x; xa = A.x; xb = B.x; xc = C.x;
        y = P.y; ya = A.y; yb = B.y; yc = C.y;
        gamma = (ya * x - yb * x + xb * y - xa * y + xa * yb - xb * ya) / (ya * xc - yb * xc + xb * yc - xa * yc + xa * yb - xb * ya);
        beta = (ya * x - yc * x + xc * y - xa * y + xa * yc - xc * ya) / (ya * xb - yc * xb + xc * yb - xa * yb + xa * yc - xc * ya);
        alpha = 1 - gamma - beta;
        return new Vector3(alpha, beta, gamma);
    }
    /// <summary>
    /// 计算双线性插值
    /// </summary>
    /// <returns>四个分量分别表示左下、左上、右上、右下的权重</returns>
    public static Vector4 Bilinear(float left, float right, float bottom, float up, float X, float Y)
    {
        if (left > right || bottom > up)
            throw new System.ArgumentException();
        float x, y;
        float alpha, beta, gamma, delta;
        x = (left == right) ? 0.5f : (X - left) / (right - left);
        y = (bottom == up) ? 0.5f : (Y - bottom) / (up - bottom);
        alpha = x * y;
        beta = x * (1f - y);
        gamma = (1f - x) * (1f - y);
        delta = 1 - alpha - beta - gamma;
        return new Vector4(alpha, beta, gamma, delta);
    }
}

