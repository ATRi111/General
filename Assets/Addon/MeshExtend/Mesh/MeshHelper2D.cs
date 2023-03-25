using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshExtend
{
    /// <summary>
    /// 用于便捷地创建用于2D图像的mesh
    /// </summary>
    [Serializable]
    public class MeshHelper2D : MeshHelper
    {
        public static Vector2 CalculateUV(Rect rect, Vector2 v)
        {
            return new Vector2((v.x - rect.x) / rect.width, (v.y - rect.y) / rect.height);
        }

        /// <summary>
        /// 用于生成默认的计算UV的方法
        /// </summary>
        public Func<Rect> GetRect;

        internal Vector4 DefaultGetUV(Vector2 point)
        {
            Rect rect = GetRect();
            return CalculateUV(rect, point);
        }

        /// <summary>
        /// 添加一个凸多边形
        /// </summary>
        /// <param name="sorted">顶点是否已经经过排序</param>
        /// <returns>新添加的顶点，需要为其分配position以外的属性(边数为3或4时，不会添加新的顶点)</returns>
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
        /// 添加一个凸多边形
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