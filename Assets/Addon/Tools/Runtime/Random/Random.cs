using System;
using System.Collections.Generic;

namespace MyTool
{
    public static class RandomTool
    {
        private static readonly Dictionary<ERandomGrounp, RandomGroup> groupDict;

        static RandomTool()
        {
            groupDict = new();
            foreach (ERandomGrounp key in Enum.GetValues(typeof(ERandomGrounp)))
            {
                groupDict.Add(key, new RandomGroup());
            }
        }

        public static RandomGroup GetGroup(ERandomGrounp key)
        {
            return groupDict[key];
        }

        private static int[] factorials;

        /// <summary>
        /// 获取numCount个数的全排列中的第index个排列（全排列按从小到大的顺序排）;
        /// <para>index取-1，则表示随机获取一个排列</para>
        /// </summary>
        public static List<int> GetPermutation(int numCount, int index = -1)
        {
            factorials = new int[numCount + 1];
            factorials[0] = 1;
            for (int i = 1; i <= numCount; i++)
            {
                factorials[i] = factorials[i - 1] * i;
            }
            List<int> numbers = new();
            for (int i = 0; i < numCount; i++)
            {
                numbers.Add(i);
            }
            List<int> ret = new();
            if (index != -1)
                index %= factorials[^1];
            else
                index = UnityEngine.Random.Range(0, factorials[^1]);

            //依次确定第i位上的值
            for (int i = 0; i < numCount; i++)
            {
                int a = factorials[numbers.Count - 1];
                ret.Add(numbers[index / a]);
                numbers.RemoveAt(index / a);
                index %= a;
            }
            return ret;
        }
    }
}