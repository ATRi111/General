using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshExtend
{
    public static class VertexUtility
    {
        /// <summary>
        /// 给List中的每个Vertex设定UV
        /// </summary>
        /// <param name="resultOnly">是否保留运算结果而清空GetUV</param>
        public static void SetUV(this List<Vertex> vertices, Func<Vector3, Vector2> GetUV, bool resultOnly = false)
        {
            if (resultOnly)
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i].uv = GetUV(vertices[i].position);
                    vertices[i].GetUV = null;
                }
            }
            else
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i].GetUV = GetUV;
                }
            }
        }

        /// <summary>
        /// 给List中的每个Vertex设定Color
        /// </summary>
        /// <param name="resultOnly">是否保留运算结果并清空GetColor</param>
        public static void SetColor(this List<Vertex> vertices, Func<Vector3, Color> GetColor, bool resultOnly = false)
        {
            if (resultOnly)
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i].color = GetColor(vertices[i].position);
                    vertices[i].GetColor = null;
                }
            }
            else
            {
                for (int i = 0; i < vertices.Count; i++)
                {
                    vertices[i].GetColor = GetColor;
                }
            }
        }

        /// <summary>
        /// 给List中的每个Vertex设定Color
        /// </summary>
        /// <param name="resultOnly">是否保留运算结果并清空GetColor</param>
        public static void SetColor(this List<Vertex> vertices, Color color)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].color = color;
            }
        }
    }
}