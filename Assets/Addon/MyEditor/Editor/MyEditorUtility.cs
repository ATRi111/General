using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace MyEditor
{
    public static class MyEditorUtility
    {
        /// <summary>
        /// ��ȡ�ʲ������ļ��е�·��(ĩβ��'/')
        /// </summary>
        public static string GetAssetFolder(UnityEngine.Object asset)
        {
            string ret = AssetDatabase.GetAssetPath(asset);
            int i = ret.LastIndexOf('/');
            ret = ret[..(i + 1)];
            return ret;
        }
        /// <summary>
        /// ����ĳ����Ľű��ļ�����Ҫȷ�������ͽű��ļ���һ�£�
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
        /// ��ȡSprite�İ�Χ�У��������������Ϸ����ı������꣩
        /// </summary>
        public static Rect GetSpriteAABB(Sprite sprite)
        {
            Vector2 size = sprite.rect.size / sprite.pixelsPerUnit;
            Vector2 pivot = new Vector2(sprite.pivot.x / sprite.texture.width, sprite.pivot.y / sprite.texture.height);
            Vector2 position = -new Vector2(pivot.x * size.x, pivot.y * size.y);
            return new Rect(position, size);
        }

        /// <summary>
        /// ��ȡSprite��������״���������������Ϸ����ı������꣬��������ֻ�е���������״��ͼƬ��
        /// </summary>
        public static List<Vector2> GetSpritePhysicsShape(Sprite sprite)
        {
            List<Vector2> ret = new List<Vector2>();
            sprite.GetPhysicsShape(0, ret);
            return ret;
        }
    }
}