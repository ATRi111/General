using UnityEngine;

namespace Tools
{
    public static partial class GeometryTool
    {
        /// <summary>
        /// 求一点到直线的垂向量（共线时，返回零向量）
        /// </summary>
        public static Vector3 NormalVector(this Ray ray, Vector3 point)
        {
            Vector3 v = point - ray.origin;
            Vector3 cross = Vector3.Cross(v, ray.direction);
            Vector3 n = Vector3.Cross(cross, ray.direction).normalized;
            float distance = Vector3.Dot(v, n);
            return distance * n;
        }

        /// <summary>
        /// 计算某点到某射线原点的向量在该射线方向上的投影的长度
        /// </summary>
        public static float DistanceOnDirection(this Ray ray, Vector3 point)
        {
            Vector3 v = point - ray.origin;
            return Vector3.Dot(v, ray.direction);
        }
        /// <summary>
        /// 过指定点作与射线方向垂直的平面，返回与射线的交点
        /// </summary>
        public static Vector3 GetPoint(this Ray ray, Vector3 worldPosition)
        {
            return ray.GetPoint(ray.DistanceOnDirection(worldPosition));
        }

        /// <summary>
        /// 求两直线上到对方最近的点
        /// </summary>
        /// <returns>两直线距离</returns>
        public static float ClosestPointPair(this Ray ray1, Ray ray2, out Vector3 point1, out Vector3 point2)
        {
            Vector3 n = Vector3.Cross(ray1.direction, ray2.direction).normalized;   //公垂线
            if (n == Vector3.zero)  //两直线平行
            {
                point1 = ray1.origin;
                point2 = ray2.origin;
                return 0f;
            }

            float cos = Vector3.Dot(ray1.direction, ray2.direction);
            Vector3 v = ray1.origin - ray2.origin;
            float sign = Mathf.Sign(Vector3.Dot(v, n));
            float distance = Vector3.Dot(n, v);
            //过一条直线上任意一点，沿公垂线作长度为两直线间距离的线段
            Vector3 projection = ray2.origin + sign * distance * n;
            Vector3 tempn = ray1.NormalVector(projection);
            float tempd = tempn.magnitude / Mathf.Sqrt(1 - cos * cos);
            point1 = projection + tempd * ray2.direction;

            projection = ray1.origin - sign * distance * n;
            tempn = ray2.NormalVector(projection);
            tempd = tempn.magnitude / Mathf.Sqrt(1 - cos * cos);
            point2 = projection + tempd * ray1.direction;
            return (point1 - point2).magnitude;
        }
    }
}

