using System.Collections.Generic;

namespace Tools
{
    public static partial class GeneralTool
    {
        public static void AddNum<TKey>(this Dictionary<TKey, int> dict, TKey key, int num = 1)
        {
            if (key != null)
            {
                if (dict.ContainsKey(key))
                    dict[key] += num;
                else
                    dict.Add(key, num);
            }
        }

        public static void RemoveNum<TKey>(this Dictionary<TKey, int> dict, TKey key, int num = 1)
        {
            if (dict.ContainsKey(key))
            {
                dict[key] -= num;
                if (dict[key] <= 0)
                    dict.Remove(key);
            }
        }
    }

}
