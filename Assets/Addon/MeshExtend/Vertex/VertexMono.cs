using System.Collections.Generic;
using UnityEngine;

namespace MeshExtend
{
    public abstract class VertexMono : MonoBehaviour
    {
        [SerializeField]
        protected Vertex vertex;

        public Vertex Vertex => vertex;

        public static List<Vertex> GetVertices(VertexMono[] monos)
        {
            List<Vertex> ret = new List<Vertex>();
            for (int i = 0; i < monos.Length; i++)
            {
                ret.Add(monos[i].vertex);
            }
            return ret;
        }

    }
}