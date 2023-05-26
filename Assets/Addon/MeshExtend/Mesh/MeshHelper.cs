using System;
using System.Collections.Generic;
using UnityEngine;

namespace MeshExtend
{
    /// <summary>
    /// ���ڱ�ݵش���mesh
    /// </summary>
    [Serializable]
    public class MeshHelper
    {
        //�����ظ����ͬһ��λ��Ϊ���㣬��ʹ���ֵ���
        internal readonly Dictionary<Vector3, int> vertexToIndex;

        public bool dirty;

        [SerializeField]
        internal protected List<Vertex> vertices;
        public List<Vertex> GetVertices()
        {
            List<Vertex> ret = new List<Vertex>();
            ret.AddRange(vertices);
            return ret;
        }
        public bool IsEmpty => vertices.Count == 0;
        public int VertexCount => vertices.Count;

        [SerializeField]
        internal protected List<int> triangles;
        public List<int> GetTriangles()
        {
            List<int> ret = new List<int>();
            ret.AddRange(triangles);
            return ret;
        }
        public int TriangleCount => triangles.Count / 3;

        public MeshHelper()
        {
            vertexToIndex = new Dictionary<Vector3, int>();
            vertices = new List<Vertex>();
            triangles = new List<int>();
        }

        public MeshHelper(Mesh mesh) : this()
        {
            for (int i = 0; i < mesh.vertices.Length; i++)
            {
                vertices.Add(new Vertex(mesh.vertices[i]));
            }
            triangles.AddRange(mesh.triangles);
        }

        public void ToMesh(Mesh mesh)
        {
            mesh.Clear();
            Vector3[] v = new Vector3[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                v[i] = vertices[i].position;
            }
            mesh.vertices = v;
            int[] t = new int[triangles.Count];
            for (int i = 0; i < triangles.Count; i++)
            {
                t[i] = triangles[i];
            }
            mesh.triangles = t;
            Vector2[] uv = new Vector2[vertices.Count];
            for (int i = 0; i < vertices.Count; i++)
            {
                uv[i] = vertices[i].uv;
            }
            mesh.uv = uv;
        }

        /// <summary>
        /// ���һ�������Σ�����ȷ����˳ʱ��˳������������
        /// </summary>
        public void AddTriangle_Sorted(Vertex v1, Vertex v2, Vertex v3)
        {
            void AddVertex(Vertex v)
            {
                int index;
                if (vertexToIndex.ContainsKey(v.position))
                    index = vertexToIndex[v.position];
                else
                {
                    index = vertices.Count;
                    vertices.Add(v);
                    vertexToIndex.Add(v.position, index);
                }
                triangles.Add(index);
            }

            AddVertex(v1);
            AddVertex(v2);
            AddVertex(v3);
            dirty = true;
        }

        /// <summary>
        /// ���һ�������Σ�����ȷ����˳ʱ��˳������������
        /// </summary>
        public void AddTriangle_Sorted(List<Vertex> tri)
        {
            if (tri.Count != 3)
                throw new ArgumentException();
            AddTriangle_Sorted(tri[0], tri[1], tri[2]);
        }

        /// <summary>
        /// ���һ���ı��Σ�����ȷ����˳ʱ��˳�����ĸ�����
        /// </summary>
        public void AddQuad_Sorted(Vertex v1, Vertex v2, Vertex v3, Vertex v4)
        {
            AddTriangle_Sorted(v1, v2, v4);
            AddTriangle_Sorted(v2, v3, v4);
        }

        /// <summary>
        /// ���һ���ı��Σ�����ȷ����˳ʱ��˳�����ĸ�����
        /// </summary>
        public void AddQuad_Sorted(List<Vertex> quad)
        {
            if (quad.Count != 4)
                throw new ArgumentException();
            AddQuad_Sorted(quad[0], quad[1], quad[2], quad[3]);
        }

        public void Clear()
        {
            vertices.Clear();
            triangles.Clear();
            vertexToIndex.Clear();
            dirty = true;
        }
    }
}
