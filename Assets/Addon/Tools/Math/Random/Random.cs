using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static class RandomTool
    {
        private static readonly Dictionary<ERandomGrounp, System.Random> groups = new Dictionary<ERandomGrounp, System.Random>();

        static RandomTool()
        {
            foreach (ERandomGrounp key in Enum.GetValues(typeof(ERandomGrounp)))
            {
                groups.Add(key, new System.Random(DateTime.Now.Second));
            }
            spare = -1;
        }

        public static void Randomize(int seed, ERandomGrounp eRandomGrounp = ERandomGrounp.Default)
        {
            groups[eRandomGrounp] = new System.Random(seed);
        }

        /// <summary>
        /// ����[min,max������������
        /// </summary>
        public static int RandomInt(int min, int max, ERandomGrounp eRandomGrounp = ERandomGrounp.Default)
            => groups[eRandomGrounp].Next(min, max);

        /// <summary>
        /// ����[min,max��������������
        /// </summary>
        public static float RandomFloat(float min, float max, ERandomGrounp eRandomGrounp = ERandomGrounp.Default)
            => (float)(groups[eRandomGrounp].NextDouble()) * (max - min) + min;

        /// <summary>
        /// ���������λ��ά������ʹ�ö���������
        /// </summary>
        public static Vector2 RandomVector2(ERandomGrounp eRandomGrounp = ERandomGrounp.Default)
        {
            double x, y, r;
            do
            {
                x = groups[eRandomGrounp].NextDouble() * 2 - 1;
                y = groups[eRandomGrounp].NextDouble() * 2 - 1;
                r = x * x + y * y;
            } while (r == 0 || r >= 1);
            return new Vector2((float)x, (float)y).normalized;
        }

        /// <summary>
        /// ���������λ��ά������ʹ�ö���������
        /// </summary>
        public static Vector3 RandomVector3(ERandomGrounp eRandomGrounp = ERandomGrounp.Default)
        {
            double x, y, z, r;
            do
            {
                x = groups[eRandomGrounp].NextDouble() * 2 - 1;
                y = groups[eRandomGrounp].NextDouble() * 2 - 1;
                z = groups[eRandomGrounp].NextDouble() * 2 - 1;
                r = x * x + y * y + z * z;
            } while (r == 0 || r >= 1);
            return new Vector2((float)x, (float)y).normalized;
        }

        private static double spare;    //�������̬�ֲ������
        /// <summary>
        /// ���ɷ��ϱ�׼��̬�ֲ��������(ʹ�ö�������)
        /// </summary>
        /// <returns></returns>
        public static double RandomNormalDistribution(ERandomGrounp eRandomGrounp = ERandomGrounp.Default)
        {
            if (spare != -1)
            {
                double ret = spare;
                spare = -1;
                return ret;
            }
            double x, y, r;
            do
            {
                x = groups[eRandomGrounp].NextDouble() * 2 - 1;
                y = groups[eRandomGrounp].NextDouble() * 2 - 1;
                r = x * x + y * y;
            } while (r == 0 || r >= 1);
            r = Math.Sqrt(-2d * Math.Log(r) / r);
            spare = y * r;
            return x * r;
        }
    }
}