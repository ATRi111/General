using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshExtend
{
    /// <summary>
    /// ���ڱ�ݵش�������2Dͼ���mesh
    /// </summary>
    [Serializable]
    public class MeshHelper2D : MeshHelper
    {
        public static Vector2 CalculateUV(Rect rect, Vector2 v)
        {
            return new Vector2((v.x - rect.x) / rect.width, (v.y - rect.y) / rect.height);
        }

        /// <summary>
        /// ��������Ĭ�ϵļ���UV�ķ���
        /// </summary>
        public Func<Rect> GetRect;

        internal Vector4 DefaultGetUV(Vector2 point)
        {
            Rect rect = GetRect();
            return CalculateUV(rect, point);
        }

        /// <summary>
        /// ���һ��͹�����
        /// </summary>
        /// <param name="sorted">�����Ƿ��Ѿ���������</param>
        /// <returns>����ӵĶ��㣬��ҪΪ�����position���������(����Ϊ3��4ʱ����������µĶ���)</returns>
        public Vertex AddConvexPolygon(List<Vertex> vertices, bool sorted = false)
        {
            int n = vertices.Count;
            if (n < 3)
                throw new ArgumentException();
            else if (n == 3)
            {
                if (!sorted)
                    vertices.SortClockWise();
                AddTriangle_Sorted(vertices);
                return null;
            }
            else if (n == 4)
            {
                if (!sorted)
                    vertices.SortClockWise();
                AddQuad_Sorted(vertices);
                return null;
            }
            else
            {
                List<List<Vertex>> tris = new List<List<Vertex>>();
                Vertex addition = MeshExtendUtility.DivideConvexPolygon(vertices, tris, sorted);
                for (int i = 0; i < tris.Count; i++)
                {
                    AddTriangle_Sorted(tris[i]);
                }
                return addition;
            }
        }

        /// <summary>
        /// ���һ��͹�����
        /// </summary>
        public Vertex AddConvexPolygon(params Vertex[] vertices)
        {
            List<Vertex> temp = new List<Vertex>();
            for (int i = 0; i < vertices.Length; i++)
            {
                temp.Add(vertices[i]);
            }
            return AddConvexPolygon(temp);
        }
    }
}