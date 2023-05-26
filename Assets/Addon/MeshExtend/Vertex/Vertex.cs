using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshExtend
{
    [Serializable]
    public class Vertex
    {
        public static readonly Color DefaultColor = Color.white;
        public static readonly Vector3 DefaultNormal = Vector3.back;
        public static readonly Vector4 DefaultTangent = new Vector4(1f, 0f, 0f, -1f);

        public Vector3 position;

        /// <summary>
        /// 仅在GetColor为空时，使用此字段
        /// </summary>
        public Color color;
        public Func<Vector3, Color> GetColor;
        public Color Color
        {
            get
            {
                if (GetColor != null)
                    color = GetColor(position);
                return color;
            }
        }

        /// <summary>
        /// 仅在GetUV为空时，使用此字段
        /// </summary>
        public Vector2 uv;
        /// <summary>
        /// 纹理映射
        /// </summary>
        public Func<Vector3, Vector2> GetUV;
        public Vector2 UV
        {
            get
            {
                if (GetUV != null)
                    uv = GetUV(position);
                return uv;
            }
        }

        public Vector2 uv2;
        public Vector2 uv3;
        public Vector2 uv4;

        public Vector3 normal;
        public Vector4 tangent;

        public Vertex()
        {
            color = DefaultColor;
            normal = DefaultNormal;
            tangent = DefaultTangent;
        }
        public Vertex(Vector3 position) : this()
        {
            this.position = position;
        }
        public Vertex(Vector3 position, Vector2 uv) : this(position)
        {
            this.uv = uv;
        }
        public Vertex(Vector3 position, Func<Vector3, Vector2> GetUV) : this(position)
        {
            this.GetUV = GetUV;
            uv = GetUV(position);
        }
        public Vertex(UIVertex vertex)
        {
            position = vertex.position;
            uv = vertex.uv0;
            uv2 = vertex.uv1;
            uv3 = vertex.uv2;
            uv4 = vertex.uv3;
            normal = vertex.normal;
            tangent = vertex.tangent;
        }
        public UIVertex ToUIVertex()
        {
            return new UIVertex()
            {
                position = position,
                color = Color,
                uv0 = UV,
                uv1 = uv2,
                uv2 = uv3,
                uv3 = uv4,
                normal = normal,
                tangent = tangent,
            };
        }

        public static List<Vertex> GenerateVertices(List<Vector3> positions)
        {
            List<Vertex> ret = new List<Vertex>();
            for (int i = 0; i < positions.Count; i++)
            {
                ret.Add(new Vertex(positions[i]));
            }
            return ret;
        }
    }
}