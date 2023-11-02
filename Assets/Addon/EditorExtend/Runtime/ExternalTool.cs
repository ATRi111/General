using System.Collections.Generic;
using UnityEngine;

namespace EditorExtend
{
    public static class ExternalTool
    {
        /// <summary>
        /// 计算指定射线起点到指定点的向量在该射线方向上的投影的长度
        /// </summary>
        public static float DistanceOnDirection(Ray ray, Vector3 point)
        {
            Vector3 v = point - ray.origin;
            return Vector3.Dot(v, ray.direction);
        }
        /// <summary>
        /// 求指定点在射线上的投影点
        /// </summary>
        public static Vector3 GetPointOnRay(Ray ray, Vector3 worldPosition)
        {
            float distance = DistanceOnDirection(ray, worldPosition);
            return ray.GetPoint(distance);  //参数可以取负数
        }
        /// <summary>
        /// 求指定射线上坐标的z分量为指定值的点
        /// </summary>
        public static Vector3 GetPointOnRay(Ray ray, float z)
        {
            float k = (z - ray.origin.z) / ray.direction.z;
            float x = ray.origin.x + k * ray.direction.x;
            float y = ray.origin.y + k * ray.direction.y;
            return new Vector3(x, y, z);
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