using System;
using System.Collections.Generic;
using UnityEngine;

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
    }
}