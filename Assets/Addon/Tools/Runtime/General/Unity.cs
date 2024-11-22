using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MyTool
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
            GameObject obj = disable ? FindGameObject(obj_name) : GameObject.Find(obj_name);
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

        public static void SetLossyScale(this Transform transform,Vector3 lossyScale)
        {
            if (transform.parent == null)
            {
                transform.localScale = lossyScale;
            }
            else
            {
                Vector3 temp = transform.parent.lossyScale;
                transform.localScale = new Vector3(lossyScale.x / temp.x, lossyScale.y / temp.y, lossyScale.z / temp.z);
            }
        }

        /// <summary>
        /// 根据CreateAssetMenuAttribute自动创建ScriptableObject
        /// </summary>
        /// <param name="path">从"Assets"开始的路径,不含文件名，末尾不含"/"</param>
        public static ScriptableObject CreateScriptableObject(Type type, string path, bool save = true)
        {
            CreateAssetMenuAttribute attribute = type.GetTypeInfo().GetCustomAttribute<CreateAssetMenuAttribute>();
            ScriptableObject so = null;
            if (attribute != null)
            {
                so = ScriptableObject.CreateInstance(type);
                AssetDatabase.CreateAsset(so, path + $"/{attribute.fileName}.asset");
            }
            if(save)
                AssetDatabase.SaveAssets();
            return so;
        }
    }
}

