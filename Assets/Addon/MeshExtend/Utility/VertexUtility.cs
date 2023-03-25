using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshExtend
{
    public static class VertexUtility
    {
        /// <summary>
        /// ��List�е�ÿ��Vertex�趨UV
        /// </summary>
        /// <param name="resultOnly">�Ƿ��������������GetUV</param>
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
        /// ��List�е�ÿ��Vertex�趨Color
        /// </summary>
        /// <param name="resultOnly">�Ƿ��������������GetColor</param>
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
        /// ��List�е�ÿ��Vertex�趨Color
        /// </summary>
        /// <param name="resultOnly">�Ƿ��������������GetColor</param>
        public static void SetColor(this List<Vertex> vertices, Color color)
        {
            for (int i = 0; i < vertices.Count; i++)
            {
                vertices[i].color = color;
            }
        }
    }
}