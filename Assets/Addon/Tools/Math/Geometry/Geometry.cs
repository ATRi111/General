using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static partial class GeometryTool
    {
        //�����еĽǶȵĺ��壺������0��1����Ӧ0�㣬����(-1,0)��Ӧ90��
        //�������£���������ĳ����λδ��תʱ���ϣ�ŷ���Ǻ�����ò�λ�ĳ����һһ��Ӧ

        /// <summary>
        /// ʹ�Ƕ�����[0��,360��)��
        /// </summary>
        public static float ClampAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0f)
                angle += 360f;
            return angle;
        }

        /// <summary>
        /// ʹ�Ƕ�����[0��,180��]��
        /// </summary>
        public static float ClampIncludedAngle(float angle)
        {
            angle = ClampAngle(angle);
            if (angle > 180f)
                angle = 360f - angle;
            return angle;
        }

        public static Vector3 CalculateCenter(params Vector3[] points)
        {
            Vector3 ret = Vector3.zero;
            foreach (Vector3 point in points)
            {
                ret += point;
            }
            ret /= points.Length;
            return ret;
        }
        public static Vector2 CalculateCenter(params Vector2[] points)
        {
            Vector2 ret = Vector2.zero;
            foreach (Vector2 point in points)
            {
                ret += point;
            }
            ret /= points.Length;
            return ret;
        }

        /// <summary>
        /// �������Ļ���͹����Σ��˷���������һ������
        /// </summary>
        /// <param name="points">�߽�ĵ㣬����Ҫ��˳���룬�˷�����outline�ᱻ�ı�</param>
        /// <param name="triangles">���ս�������������飬ԭ�������ݲ��ᱻ���</param>
        /// <param name="index">������������±�Ӽ���ʼ</param>
        public static void DivideConvexPolygon_Center(List<Vector2> points, List<int> triangles, int index = 0)
        {
            int n = points.Count;
            if (n < 3)
                throw new ArgumentException();

            Vector2 center = CalculateCenter(points.ToArray());
            Comparer_Vector2_Clockwise comparer = new Comparer_Vector2_Clockwise(center);
            points.Sort(comparer);
            points.Add(center);
            for (int i = 0; i < n - 1; i++)
            {
                triangles.Add(i + index);
                triangles.Add(i + 1 + index);
                triangles.Add(n + index);
            }
            triangles.Add(n - 1 + index);
            triangles.Add(index);
            triangles.Add(n + index);
        }

        /// <summary>
        /// �������Ļ���͹����Σ��˷���������һ������
        /// </summary>
        /// <param name="points">�߽�ĵ㣬����Ҫ��˳���룬�˷�����outline�ᱻ�ı�</param>
        /// <param name="triangles">���ս��,���η���ÿ�������Σ�˳ʱ�뷵�������ε����������꣩</param>
        public static void DivideConvexPolygon_Center(List<Vector2> points, List<List<Vector2>> triangles)
        {
            int n = points.Count;
            if (n < 3)
                throw new ArgumentException();

            triangles.Clear();
            Vector2 center = CalculateCenter(points.ToArray());
            Comparer_Vector2_Clockwise comparer = new Comparer_Vector2_Clockwise(center);
            points.Sort(comparer);
            points.Add(center);
            for (int i = 0; i < n - 1; i++)
            {
                triangles.Add(new List<Vector2> { points[i], points[i + 1], points[n] });
            }
            triangles.Add(new List<Vector2> { points[n - 1], points[0], points[n] });
        }
    }
}

