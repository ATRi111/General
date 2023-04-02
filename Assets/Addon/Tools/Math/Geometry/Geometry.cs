using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
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
        /// 利用中心划分凸多边形，此方法会增加一个顶点
        /// </summary>
        /// <param name="points">边界的点，不需要按顺序传入，此方法中outline会被改变</param>
        /// <param name="triangles">接收结果的三角形数组，原本的内容不会被清除</param>
        /// <param name="index">三角形数组的下标从几开始</param>
        public static void DivideConvexPolygon_Center(List<Vector2> points, List<int> triangles, int index = 0)
        {
            int n = points.Count;
            if (n < 3)
                throw new ArgumentException();

            Vector2 center = CalculateCenter(points.ToArray());
            Comparer_Vector2_Clockwise comparer = new Comparer_Vector2_Clockwise(center);
            points.Sort(comparer);
            points.Add(center);
            for (int i = 0; i < n - 1; i++)
            {
                triangles.Add(i + index);
                triangles.Add(i + 1 + index);
                triangles.Add(n + index);
            }
            triangles.Add(n - 1 + index);
            triangles.Add(index);
            triangles.Add(n + index);
        }

        /// <summary>
        /// 利用中心划分凸多边形，此方法会增加一个顶点
        /// </summary>
        /// <param name="points">边界的点，不需要按顺序传入，此方法中outline会被改变</param>
        /// <param name="triangles">接收结果,依次返回每个三角形（顺时针返回三角形的三个点坐标）</param>
        public static void DivideConvexPolygon_Center(List<Vector2> points, List<List<Vector2>> triangles)
        {
            int n = points.Count;
            if (n < 3)
                throw new ArgumentException();

            triangles.Clear();
            Vector2 center = CalculateCenter(points.ToArray());
            Comparer_Vector2_Clockwise comparer = new Comparer_Vector2_Clockwise(center);
            points.Sort(comparer);
            points.Add(center);
            for (int i = 0; i < n - 1; i++)
            {
                triangles.Add(new List<Vector2> { points[i], points[i + 1], points[n] });
            }
            triangles.Add(new List<Vector2> { points[n - 1], points[0], points[n] });
        }
    }
}

