using System.Collections.Generic;
using UnityEngine;
using static MeshExtend.MeshExtendUtility;

namespace MeshExtend
{
    public static class VertexUtility2D
    {
        public static void SortClockWise(this List<Vertex> vertices)
        {
            Vertex center = GenerateCenter(vertices.ToArray());
            Comparer_Clockwise comparer = new Comparer_Clockwise(center.position);
            vertices.Sort(comparer);
        }

        /// <summary>
        /// ����������Σ�������
        /// </summary>
        /// <param name="center">����</param>
        /// <param name="r">���Բ�뾶</param>
        /// <param name="n">����</param>
        /// <param name="rotateAngle">��ת��</param>
        public static List<Vertex> GenerateRegularPolygon(Vector2 center, float r, int n, float rotateAngle = 0f)
        {
            List<Vertex> ret = new List<Vertex>();
            float deltaAngle = 360f / n;
            for (int i = 0; i < n; i++, rotateAngle += deltaAngle)
            {
                ret.Add(new Vertex(center + r * ToDirection(rotateAngle)));
            }
            return ret;
        }

        /// <summary>
        /// �����п�ȵ��߶Σ�������
        /// </summary>
        /// <param name="extend">�����Ƿ�Ҫ�ֱ��ӳ�width/2</param>
        public static List<Vertex> GenerateLine(Vector2 a, Vector2 b, float width, bool extend = false)
        {
            Vector2 direction = (b - a).normalized;
            Vector2 normal = new Vector2(direction.y, -direction.x);
            float halfWidth = width / 2;
            if (extend)
            {
                a -= halfWidth * direction;
                b += halfWidth * direction;
            }
            List<Vector3> outline = new List<Vector3>
            {
                a + halfWidth * normal,
                a - halfWidth * normal,
                b + halfWidth * normal,
                b - halfWidth * normal
            };
            List<Vertex> ret = Vertex.GenerateVertices(outline);
            Vertex center = GenerateCenter(ret.ToArray());
            Comparer_Clockwise comparer = new Comparer_Clockwise(center.position);
            ret.Sort(comparer);
            return ret;
        }

        /// <summary>
        /// ���������ľ��Σ�������
        /// </summary>
        public static List<Vertex> GenerateRect(Rect rect)
            => GenerateRect(rect.xMin, rect.yMin, rect.xMax, rect.yMax);

        /// <summary>
        /// ���������ľ��Σ�������
        /// </summary>
        public static List<Vertex> GenerateRect(float x1, float y1, float x2, float y2)
        {
            List<Vector3> outline = new List<Vector3>()
            {
                new Vector3(x2,y2),
                new Vector3(x2,y1),
                new Vector3(x1,y1),
                new Vector3(x1,y2),
            };
            return Vertex.GenerateVertices(outline);
        }

        /// <summary>
        /// ���ɾ��ȵġ������ľ�������
        /// </summary>
        /// <param name="nx">x�����ϻ��ֵĸ���</param>
        /// <param name="ny">y�����ϻ��ֵĸ���</param>
        public static List<List<Vertex>> GenerateGrid(float x1, float x2, float y1, float y2, float nx, float ny)
        {
            List<List<Vertex>> ret = new List<List<Vertex>>();
            if (x1 > x2)
                (x1, x2) = (x2, x1);
            if (y1 > y2)
                (y1, y2) = (y2, y1);
            float deltaX = (x2 - x1) / nx;
            float deltaY = (y2 - y1) / ny;

            for (float x = x1; x < x2; x += deltaX)
            {
                for (float y = y1; y < y2; y += deltaY)
                {
                    ret.Add(GenerateRect(x, y, x + deltaX, y + deltaY));
                }
            }
            return ret;
        }
    }
}