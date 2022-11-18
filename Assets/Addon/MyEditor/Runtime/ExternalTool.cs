using System.Collections.Generic;
using UnityEngine;

namespace MyEditor
{
    public static class ExternalTool
    {
        /// <summary>
        /// 计算某点到某射线原点的向量在该射线方向上的投影的长度
        /// </summary>
        public static float DistanceOnDirection(Ray ray, Vector3 point)
        {
            Vector3 v = point - ray.origin;
            return Vector3.Dot(v, ray.direction);
        }
        /// <summary>
        /// 过指定点作与射线方向垂直的平面，返回与射线的交点
        /// </summary>
        public static Vector3 GetPointOnRay(Ray ray, Vector3 worldPosition)
        {
            return ray.GetPoint(DistanceOnDirection(ray, worldPosition));
        }
        public static bool Parallel(Vector3 v1, Vector3 v2)
        {
            return Vector3.Cross(v1, v2) == Vector3.zero || Vector3.Cross(v1, -v2) == Vector3.zero;
        }

        public static Vector2 Projection(Vector2 v, Vector2 target)
        {
            if (target == Vector2.zero)
                return Vector2.zero;
            target = target.normalized;
            return target * Vector2.Dot(v, target);
        }

        public static Rect GetAABB(List<Vector3> polygon)
        {
            float left, right, up, down;
            left = down = float.MaxValue;
            right = up = float.MinValue;

            float x, y;
            for (int i = 0; i < polygon.Count; i++)
            {
                x = polygon[i].x;
                y = polygon[i].y;
                left = Mathf.Min(left, x);
                right = Mathf.Max(right, x);
                down = Mathf.Min(down, y);
                up = Mathf.Max(up, y);
            }
            return new Rect(left, down, right - left, up - down);
        }
    }
}