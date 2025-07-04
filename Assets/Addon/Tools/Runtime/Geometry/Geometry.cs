using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyTool
{
    public static partial class GeometryTool
    {
        //此类中的角度的含义：向量（0，1）对应0°，向量(-1,0)对应90°
        //此条件下，如果物体的某个部位未旋转时朝上，欧拉角和物体该部位的朝向就一一对应

        /// <summary>
        /// 使角度落在[0°,360°)内
        /// </summary>
        public static float ClampAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0f)
                angle += 360f;
            return angle;
        }

        /// <summary>
        /// 使角度落在[0°,180°]内
        /// </summary>
        public static float ClampIncludedAngle(float angle)
        {
            angle = ClampAngle(angle);
            if (angle > 180f)
                angle = 360f - angle;
            return angle;
        }

        public static Vector3 CalculateCenter(params Vector3[] points)
        {
            Vector3 ret = Vector3.zero;
            foreach (Vector3 point in points)
            {
                ret += point;
            }
            ret /= points.Length;
            return ret;
        }
        public static Vector2 CalculateCenter(params Vector2[] points)
        {
            Vector2 ret = Vector2.zero;
            foreach (Vector2 point in points)
            {
                ret += point;
            }
            ret /= points.Length;
            return ret;
        }

        /// <summary>
        /// 计算重心坐标
        /// </summary>
        /// <returns>三个分量先后表示A、B、C的权重</returns>
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
    }
}

