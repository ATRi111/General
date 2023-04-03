using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static partial class GeometryTool
    {
        private readonly static List<Vector3> circlePoints = new List<Vector3>();

        static GeometryTool()
        {
            for (float i = 0; i < 360; i++)
            {
                circlePoints.Add(i.ToDirection());
            }
        }

        /// <param name="count">必须是360的因数</param>
        public static void GetcirclePoints(Vector3 center, float radius, int count, List<Vector3> ret)
        {
            int delta = 360 / count;
            if (count * delta != 360)
                throw new System.ArgumentException();
            for (int i = 0; i < 360; i += delta)
            {
                ret.Add(center + radius * circlePoints[i]);
            }
        }

        /// <summary>
        /// 角度转二维矢量
        /// </summary>
        public static Vector2 ToDirection(this float angle)
            => new Vector2(-Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        /// <summary>
        /// 二维矢量转角度[0°,360°)
        /// </summary>
        public static float ToAngle(this Vector2 direction)
        {
            float angle = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            return ClampAngle(angle);
        }

        /// <summary>
        /// 求v在target上的投影
        /// </summary>
        public static Vector3 Projection(this Vector3 v, Vector3 target)
        {
            if (target == Vector3.zero)
                return Vector3.zero;
            target = target.normalized;
            return target * Vector3.Dot(v, target);
        }
        /// <summary>
        /// 求v在target上的投影
        /// </summary>
        public static Vector2 Projection(this Vector2 v, Vector2 target)
        {
            return Projection((Vector3)v, (Vector3)target);
        }

        public static bool Parallel(this Vector3 v1, Vector3 v2)
        {
            return Vector3.Cross(v1, v2) == Vector3.zero || Vector3.Cross(v1, -v2) == Vector3.zero;
        }

        /// <summary>
        /// 对向量按与某个向量的夹角大小排序
        /// </summary>
        public class Comparer_Vector2_Nearer : IComparer<Vector2>, IComparer<Vector2Int>
        {
            public Vector2 direciton;

            public Comparer_Vector2_Nearer(Vector2 direciton)
            {
                this.direciton = direciton;
            }

            public int Compare(Vector2 x, Vector2 y)
            {
                float angleX = Vector2.Angle(direciton, x);
                float angleY = Vector2.Angle(direciton, y);
                return angleX.CompareTo(angleY);
            }

            public int Compare(Vector2Int x, Vector2Int y)
            {
                return Compare((Vector2)x, (Vector2)y);
            }
        }

        /// <summary>
        /// 以所给点为原点，对点按顺时针排序
        /// </summary>
        public class Comparer_Vector2_Clockwise : IComparer<Vector2>, IComparer<Vector2Int>, IComparer<Vector3>
        {
            public Vector2 center;

            public Comparer_Vector2_Clockwise(Vector2 _center)
            {
                center = _center;
            }

            public int Compare(Vector2 x, Vector2 y)
            {
                float angleX = (x - center).ToAngle();
                float angleY = (y - center).ToAngle();
                return angleY.CompareTo(angleX);
            }
            public int Compare(Vector2Int x, Vector2Int y)
            {
                return Compare((Vector2)x, (Vector2)y);
            }
            public int Compare(Vector3 x, Vector3 y)
            {
                return Compare((Vector2)x, (Vector2)y);
            }
        }

        public static Rect GetBoundArea(List<Vector3> points)
        {
            if (points.Count == 0)
                return default;
            float xMin, xMax, yMin, yMax;
            xMin = xMax = points[0].x;
            yMin = yMax = points[0].y;
            for (int i = 1; i < points.Count; i++)
            {
                if (points[i].x < xMin)
                    xMin = points[i].x;
                else if (points[i].x > xMax)
                    xMax = points[i].x;
                if (points[i].y < yMin)
                    yMin = points[i].y;
                else if (points[i].y > yMax)
                    yMax = points[i].y;
            }
            return new Rect(xMin, yMin, xMax - xMin, yMax - yMin);
        }

        /// <summary>
        /// 方向向量的插值
        /// </summary>
        public static Vector3 DirectionLerp(Vector3 v1, Vector3 v2, float k)
        {
            Quaternion q1 = Quaternion.LookRotation(v1);
            Quaternion q2 = Quaternion.LookRotation(v2);
            Quaternion q = Quaternion.Lerp(q1, q2, k);
            return (q * Vector3.forward).normalized;
        }
    }
}

