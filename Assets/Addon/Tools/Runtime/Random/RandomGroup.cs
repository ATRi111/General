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
        /// 返回[min,max）间的随机整数
        /// </summary>
        public int NextInt(int min, int max)
            => random.Next(min, max);

        /// <summary>
        /// 返回[min,max）间的随机浮点数
        /// </summary>
        public float RandomFloat(float min, float max)
            => (float)(random.NextDouble()) * (max - min) + min;

        /// <summary>
        /// 生成随机单位二维向量（使用多个随机数）
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
        /// 生成随机单位三维向量（使用多个随机数）
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

        private static double spare;    //多余的正态分布随机数
        /// <summary>
        /// 生成符合标准正态分布的随机数(使用多个随机数)
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