using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public static partial class GeneralTool
    {
        /// <summary>
        /// 查找游戏物体，可以找到被禁用的游戏物体
        /// </summary>
        public static GameObject FindGameObject(string obj_name)
        {
            UnityEngine.Object[] objs = Resources.FindObjectsOfTypeAll(typeof(GameObject));
            foreach (UnityEngine.Object obj in objs)
            {
                if (obj.name == obj_name)
                    return obj as GameObject;
            }
            return null;
        }

        /// <summary>
        /// 按游戏物体名查找组件
        /// </summary>
        /// <param name="disable">要查找的组件所在的游戏物体是否可能被禁用</param>
        public static T FindComponent<T>(string obj_name, bool disable = false) where T : Component
        {
            GameObject obj = disable ? GeneralTool.FindGameObject(obj_name) : GameObject.Find(obj_name);
            if (obj == null)
                return null;
            T ret = obj.GetComponentInChildren<T>();
            return ret;
        }
        /// <summary>
        /// 按标签查找组件
        /// </summary>
        public static T FindComponentWithTag<T>(string tag) where T : Component
        {
            GameObject obj = GameObject.FindGameObjectWithTag(tag);
            if (obj == null)
                return null;
            T ret = obj.GetComponentInChildren<T>();
            return ret;
        }

        public static void Log<T>(this List<T> list)
        {
            string s = null;
            foreach (T item in list)
            {
                s += item.ToString() + "|";
            }
            Debug.Log(s);
        }
    }
}

