using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static partial class MathTool
    {
        public const float DIAGNOL = 1.414214f;

        public static Vector3 ResetZ(this Vector3 v, float z = 0f)
            => new Vector3(v.x, v.y, z);
        public static Vector3 AddZ(this Vector2 v)
            => (Vector3)v;
        public static Vector2 RemoveZ(this Vector3 v)
            => (Vector2)v;

        /// <summary>
        /// 求贝塞尔曲线上的一点
        /// </summary>
        public static Vector3 BezierLerp(Vector3[] points, float percent)
        {
            int count = points.Length;
            switch (count)
            {
                case 0:
                case 1:
                    throw new ArgumentException();
                case 2:
                    return Vector3.Lerp(points[0], points[1], percent);
                default:
                    Vector3[] newPoints = new Vector3[count - 1];
                    for (int i = 0; i < count - 1; i++)
                    {
                        newPoints[i] = Vector3.Lerp(points[i], points[i + 1], percent);
                    }
                    return BezierLerp(newPoints, percent);
            }
        }

        /// <summary>
        /// 计算p-norm
        /// </summary>
        public static float CalculateNorm(float p, params float[] components)
        {
            float ret = 0;
            for (int i = 0; i < components.Length; i++)
            {
                ret += Mathf.Pow(components[i], p);
            }
            ret = Mathf.Pow(ret, 1f / p);
            return ret;
        }

        /// <summary>
        /// 计算高次插值
        /// </summary>
        public static float PLerp(float x, float y, float k, float p)
        {
            return x + (y - x) * Mathf.Pow(k, p);
        }

        /// <summary>
        /// 排列几种对象，输出均匀排列后的结果，用对象的序号表示
        /// </summary>
        /// <param name="nums">依次表示序号为0、1、2……的对象的数量</param>
        public static List<int> MixList(params int[] nums)
        {
            List<int> ret = new List<int>();
            int kind = nums.Length;
            int count = 0;
            for (int i = 0; i < kind; i++)
            {
                if (nums[i] < 0)
                    throw new ArgumentException();
                count += nums[i];
            }

            float[] counter = new float[kind];
            float[] percent = new float[kind];
            int maxIndex;
            ret.Clear();
            for (int i = 0; i < kind; i++)
            {
                percent[i] = nums[i] / (float)count;
            }
            for (int i = 0; i < count; i++)
            {
                maxIndex = 0;
                for (int j = 0; j < kind; j++)
                {
                    counter[j] += percent[j];
                    if (counter[j] > counter[maxIndex])
                        maxIndex = j;
                }
                counter[maxIndex]--;
                ret.Add(maxIndex);
            }
            return ret;
        }

    }
}