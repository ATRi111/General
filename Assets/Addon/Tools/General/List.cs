using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static partial class GeneralTool
    {
        public static bool TrueForAny<T>(this List<T> list, Predicate<T> match)
            => !list.TrueForAll(x => !match(x));

        public static List<Vector2Int> AllAdd(this List<Vector2Int> list, Vector2Int offset)
        {
            List<Vector2Int> ret = new List<Vector2Int>();
            foreach (Vector2Int v in list)
            {
                ret.Add(v + offset);
            }
            return ret;
        }
        public static List<Vector2> AllAdd(this List<Vector2> list, Vector2 offset)
        {
            List<Vector2> ret = new List<Vector2>();
            foreach (Vector2 v in list)
            {
                ret.Add(v + offset);
            }
            return ret;
        }
    }
}

