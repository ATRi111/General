using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshExtend
{
    public static class MeshExtendUtility
    {
        /// <summary>
        /// ����һϵ��Vertex���������ǵ����ģ����ɵ�Vertex��������position
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
        /// �Ƕ�ת��άʸ������0��1����Ӧ0�㣬(-1,0)��Ӧ90��
        /// </summary>
        public static Vector2 ToDirection(float angle)
            => new Vector2(-Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));

        /// <summary>
        /// ��άʸ��ת�Ƕȣ���0��1����Ӧ0�㣬(-1,0)��Ӧ90��
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
        /// ��������Ϊԭ�㣬�Ե㰴˳ʱ������
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
        /// �������Ļ���͹�����,�˷��������Ӷ���ε�����Ϊ�µĶ���
        /// </summary>
        /// <param name="vertices">����</param>
        /// <param name="triangles">���ս��,���η���ÿ�������Σ�˳ʱ�뷵�������ε����������꣩</param>
        /// <param name="sorted">�����Ƿ��Ѿ������</param>
        /// <returns>����ӵĶ��㣬��ҪΪ�����position���������</returns>
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