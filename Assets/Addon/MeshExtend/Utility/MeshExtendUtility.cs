using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshExtend
{
    public static class MeshExtendUtility
    {
        /// <summary>
        /// 输入一系列Vertex，生成它们的中心，生成的Vertex仅设置了position
        /// </summary>
        public static Vertex GenerateCenter(params Vertex[] vertices)
        {
            Vector3 center = Vector3.zero;
            foreach (Vertex point in vertices)
            {
                center += point.position;
            }
            center /= vertices.Length;
            return new Vertex(center);
        }

        /// <summary>
        /// 角度转二维矢量，（0，1）对应0°，(-1,0)对应90°
        /// </summary>
        public static Vector2 ToDirection(float angle)
            => new Vector2(-Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));

        /// <summary>
        /// 二维矢量转角度，（0，1）对应0°，(-1,0)对应90°
        /// </summary>
        public static float ToAngle(Vector2 direction)
        {
            float angle = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            angle %= 360f;
            if (angle < 0f)
                angle += 360f;
            return angle;
        }

        /// <summary>
        /// 以所给点为原点，对点按顺时针排序
        /// </summary>
        public class Comparer_Clockwise : IComparer<Vector2>, IComparer<Vector2Int>, IComparer<Vector3>, IComparer<Vertex>
        {
            public Vector2 center;

            public Comparer_Clockwise(Vector2 _center)
            {
                center = _center;
            }

            public int Compare(Vector2 x, Vector2 y)
            {
                return (int)Mathf.Sign(ToAngle(y - center) - ToAngle(x - center));
            }
            public int Compare(Vector2Int x, Vector2Int y)
            {
                return Compare((Vector2)x, (Vector2)y);
            }
            public int Compare(Vector3 x, Vector3 y)
            {
                return Compare((Vector2)x, (Vector2)y);
            }
            public int Compare(Vertex x, Vertex y)
            {
                return Compare(x.position, y.position);
            }
        }

        /// <summary>
        /// 利用中心划分凸多边形,此方法会增加多边形的中心为新的顶点
        /// </summary>
        /// <param name="vertices">顶点</param>
        /// <param name="triangles">接收结果,依次返回每个三角形（顺时针返回三角形的三个点坐标）</param>
        /// <param name="sorted">顶点是否已经排序过</param>
        /// <returns>新添加的顶点，需要为其分配position以外的属性</returns>
        public static Vertex DivideConvexPolygon(List<Vertex> vertices, List<List<Vertex>> triangles, bool sorted = false)
        {
            int n = vertices.Count;
            if (n < 3)
                throw new ArgumentException();

            triangles.Clear();
            Vertex center = GenerateCenter(vertices.ToArray());
            if (!sorted)
                vertices.SortClockWise();
            vertices.Add(center);
            for (int i = 0; i < n - 1; i++)
            {
                triangles.Add(new List<Vertex> { vertices[i], vertices[i + 1], vertices[n] });
            }
            triangles.Add(new List<Vertex> { vertices[n - 1], vertices[0], vertices[n] });
            return center;
        }
    }
}