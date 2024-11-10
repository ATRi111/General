using System;
using UnityEngine;

namespace MyTool
{
    public class RandomGroup
    {
        public const int MaxSeed = 1000000007;
        private System.Random random;
        private int seed;
        public int Seed
        {
            get => seed;
            set
            {
                seed = value % MaxSeed;
                random = new System.Random(seed);
            }
        }

        public RandomGroup()
        {
            spare = -1;
            Seed = (int)DateTime.Now.Ticks;
        }

        /// <summary>
        /// ����[min,max������������
        /// </summary>
        public int NextInt(int min, int max)
            => random.Next(min, max);

        /// <summary>
        /// ����[min,max��������������
        /// </summary>
        public float RandomFloat(float min, float max)
            => (float)(random.NextDouble()) * (max - min) + min;

        /// <summary>
        /// ���������λ��ά������ʹ�ö���������
        /// </summary>
        public Vector2 RandomVector2()
        {
            double x, y, r;
            do
            {
                x = random.NextDouble() * 2 - 1;
                y = random.NextDouble() * 2 - 1;
                r = x * x + y * y;
            } while (r == 0 || r >= 1);
            return new Vector2((float)x, (float)y).normalized;
        }

        /// <summary>
        /// ���������λ��ά������ʹ�ö���������
        /// </summary>
        public Vector3 RandomVector3()
        {
            double x, y, z, r;
            do
            {
                x = random.NextDouble() * 2 - 1;
                y = random.NextDouble() * 2 - 1;
                z = random.NextDouble() * 2 - 1;
                r = x * x + y * y + z * z;
            } while (r == 0 || r >= 1);
            return new Vector2((float)x, (float)y).normalized;
        }

        private static double spare;    //�������̬�ֲ������
        /// <summary>
        /// ���ɷ��ϱ�׼��̬�ֲ��������(ʹ�ö�������)
        /// </summary>
        /// <returns></returns>
        public double RandomNormalDistribution()
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
                x = random.NextDouble() * 2 - 1;
                y = random.NextDouble() * 2 - 1;
                r = x * x + y * y;
            } while (r == 0 || r >= 1);
            r = Math.Sqrt(-2d * Math.Log(r) / r);
            spare = y * r;
            return x * r;
        }
    }
}