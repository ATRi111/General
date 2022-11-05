using UnityEngine;

namespace MeshExtend
{
    public class VertexMono_UISpace : VertexMono
    {
        public void Locate(RectTransform rectTransform)
        {
            vertex.position = rectTransform.InverseTransformPoint(transform.position);
        }

        public static void LocateAll(VertexMono_UISpace[] monos, RectTransform rectTransform)
        {
            for (int i = 0; i < monos.Length; i++)
            {
                monos[i].Locate(rectTransform);
            }
        }
    }
}