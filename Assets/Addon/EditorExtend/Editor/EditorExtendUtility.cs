using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace EditorExtend
{
    public static class EditorExtendUtility
    {
        /// <summary>
        /// 拆分资产的路径
        /// </summary>
        /// <param name="path">完整路径</param>
        /// <param name="folder">到所在文件夹为止的路径(末尾含'/')</param>
        /// <param name="file">文件名(不含拓展名)</param>
        /// <param name="extend">拓展名(含点号)</param>
        public static void DevideAssetPath(in string path, out string folder, out string file, out string extend)
        {
            int i = path.LastIndexOf('/');
            int j = path.LastIndexOf('.');
            folder = path[..(i + 1)];
            file = path[(i + 1)..j];
            extend = path[j..];
        }

        /// <summary>
        /// 查找某个类的脚本文件（需要确保类名和脚本文件名一致）
        /// </summary>
        public static UnityEngine.Object FindMonoScrpit(Type type)
        {
            static string GetLast(string s, char seperator)
            {
                string[] temp = s.ToString().Split(seperator);
                return temp[temp.Length - 1];
            }

            string shortName = GetLast(type.ToString(), '.');
            string[] guids = AssetDatabase.FindAssets($"{shortName} t:Script");

            for (int i = 0; i < guids.Length; i++)
            {
                string path = AssetDatabase.GUIDToAssetPath(guids[i]);
                string fileName = GetLast(path, '/');
                if (fileName == shortName + ".cs")
                {
                    UnityEngine.Object ret = AssetDatabase.LoadAssetAtPath(path, typeof(MonoScript));
                    return ret;
                }
            }

            return null;
        }

        public static bool HasAttribute<T>(MemberInfo member, bool inherit = false) where T : Attribute
        {
            return member.GetCustomAttributes(typeof(T), inherit).Length > 0;
        }

        public static T GetAttribute<T>(MemberInfo member, bool inherit = false) where T : Attribute
        {
            object[] ret = member.GetCustomAttributes(typeof(T), inherit);
            return ret.Length > 0 ? ret[0] as T : null;
        }

        public static T ToEnum<T>(this int enumIndex) where T : Enum
            => (T)Enum.ToObject(typeof(T), enumIndex);
        public static int ToInt(this Enum e)
            => e.GetHashCode();

        /// <summary>
        /// 获取Sprite的包围盒（相对于其所在游戏物体的本地坐标）
        /// </summary>
        public static Rect GetSpriteAABB(Sprite sprite)
        {
            Vector2 size = sprite.rect.size / sprite.pixelsPerUnit;
            Vector2 pivot = new Vector2(sprite.pivot.x / sprite.texture.width, sprite.pivot.y / sprite.texture.height);
            Vector2 position = -new Vector2(pivot.x * size.x, pivot.y * size.y);
            return new Rect(position, size);
        }

        /// <summary>
        /// 获取Sprite的物理形状（相对于其所在游戏物体的本地坐标，仅适用于只有单个物理形状的图片）
        /// </summary>
        public static List<Vector2> GetSpritePhysicsShape(Sprite sprite)
        {
            List<Vector2> ret = new List<Vector2>();
            sprite.GetPhysicsShape(0, ret);
            return ret;
        }
    }
}