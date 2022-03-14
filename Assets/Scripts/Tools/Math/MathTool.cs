using System;
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
    public static Vector3 ResetZ(this Vector3 v, float z = 0f)
        => new Vector3(v.x, v.y, z);
    public static Vector3 AddZ(this Vector2 v)
        => new Vector3(v.x, v.y);
    public static Vector2 RemoveZ(this Vector3 v)
        => new Vector2(v.x, v.y);


    /// <summary>
    /// 计算重心坐标
    /// </summary>
    /// <returns>三个分量分别表示A、B、C的权重</returns>
    public static Vector3 BarycentricCoordinates(Vector3 A, Vector3 B, Vector3 C, Vector3 P)
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
    /// 求点到平面的有向距离
    /// </summary>
    /// <param name="normal">平面的法向量（平面没有旋转时）</param>
    public static float GetSignedDistance(Vector3 point, Transform plane, Vector3 normal)
    {
        normal.Normalize();
        return Vector3.Dot(plane.InverseTransformPoint(point), normal);
    }

    /// <summary>
    /// 求点在平面上的投影（返回平面坐标系下的位置）
    /// </summary>
    /// <param name="normal">平面的法向量（平面没有旋转时）</param>
    public static Vector3 Project_Local(Vector3 point, Transform plane, Vector3 normal)
    {
        float sd = GetSignedDistance(point, plane, normal);
        Vector3 local = plane.InverseTransformPoint(point);
        return local - sd * normal;
    }

    /// <summary>
    /// 求点在平面上的投影
    /// </summary>
    /// <param name="normal">平面的法向量（平面没有旋转时）</param>
    public static Vector3 Project(Vector3 point, Transform plane, Vector3 normal)
    {
        return plane.TransformPoint(Project_Local(point, plane, normal));
    }

    /// <summary>
    /// 求贝塞尔曲线上的一点
    /// </summary>
    public static Vector3 BezierLerp(Vector3[] points, float percent)
    {
        int count = points.Length;
        switch (count)
        {
            case 0:
            case 1:
                throw new ArgumentException();
            case 2:
                return Vector3.Lerp(points[0], points[1], percent);
            default:
                Vector3[] newPoints = new Vector3[count - 1];
                for (int i = 0; i < count - 1; i++)
                {
                    newPoints[i] = Vector3.Lerp(points[i], points[i + 1], percent);
                }
                return BezierLerp(newPoints, percent);
        }
    }
}

