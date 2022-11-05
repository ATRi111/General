using UnityEngine;

namespace Tools
{
    public static partial class GeometryTool
    {
        /// <summary>
        /// ��һ�㵽ֱ�ߵĴ�����������ʱ��������������
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
        /// ����ĳ�㵽ĳ����ԭ��������ڸ����߷����ϵ�ͶӰ�ĳ���
        /// </summary>
        public static float DistanceOnDirection(this Ray ray, Vector3 point)
        {
            Vector3 v = point - ray.origin;
            return Vector3.Dot(v, ray.direction);
        }
        /// <summary>
        /// ��ָ�����������߷���ֱ��ƽ�棬���������ߵĽ���
        /// </summary>
        public static Vector3 GetPoint(this Ray ray, Vector3 worldPosition)
        {
            return ray.GetPoint(ray.DistanceOnDirection(worldPosition));
        }

        /// <summary>
        /// ����ֱ���ϵ��Է�����ĵ�
        /// </summary>
        /// <returns>��ֱ�߾���</returns>
        public static float ClosestPointPair(this Ray ray1, Ray ray2, out Vector3 point1, out Vector3 point2)
        {
            Vector3 n = Vector3.Cross(ray1.direction, ray2.direction).normalized;   //������
            if (n == Vector3.zero)  //��ֱ��ƽ��
            {
                point1 = ray1.origin;
                point2 = ray2.origin;
                return 0f;
            }

            float cos = Vector3.Dot(ray1.direction, ray2.direction);
            Vector3 v = ray1.origin - ray2.origin;
            float sign = Mathf.Sign(Vector3.Dot(v, n));
            float distance = Vector3.Dot(n, v);
            //��һ��ֱ��������һ�㣬�ع�����������Ϊ��ֱ�߼������߶�
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

