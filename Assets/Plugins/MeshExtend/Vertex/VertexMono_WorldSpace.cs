using UnityEngine;

namespace MeshExtend
{
    public class VertexMono_WorldSpace : VertexMono
    {
        public void Locate(Camera camera, RectTransform rectTransform)
        {
            Vector2 screenPoint = camera.WorldToScreenPoint(transform.position);
            Vector2 localPoint = rectTransform.InverseTransformPoint(screenPoint);
            vertex.position = localPoint;
        }

        public static void LocateAll(VertexMono_WorldSpace[] monos, Camera camera, RectTransform rectTransform)
        {
            for (int i = 0; i < monos.Length; i++)
            {
                monos[i].Locate(camera, rectTransform);
            }
        }
    }
}