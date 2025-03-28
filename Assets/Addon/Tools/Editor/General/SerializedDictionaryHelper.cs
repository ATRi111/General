using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace MyTool
{
    internal static class SerializedDictionaryHelper
    {
        /// <summary>
        /// 自动补全SerializedDictionary中的元素，使之与某种Enum匹配
        /// </summary>
        /// <param name="property">SerializedDictionary中的list对应的SerializedProperty</param>
        public static void FixEnum<TKey>(SerializedProperty property) where TKey : Enum
        {
            HashSet<TKey> temp = new();
            for (int i = 0; i < property.arraySize; i++)
            {
                SerializedProperty element = property.GetArrayElementAtIndex(i);
                SerializedProperty key = element.FindPropertyRelative(nameof(key));
                temp.Add((TKey)Enum.ToObject(typeof(TKey), key.enumValueIndex));
            }
            Array array = Enum.GetValues(typeof(TKey));
            foreach (TKey e in array)
            {
                if (!temp.Contains(e))
                {
                    property.InsertArrayElementAtIndex(property.arraySize);
                    SerializedProperty element = property.GetArrayElementAtIndex(property.arraySize - 1);
                    SerializedProperty key = element.FindPropertyRelative(nameof(key));
                    key.enumValueIndex = e.GetHashCode();
                }
            }
            property.serializedObject.ApplyModifiedProperties();
        }

        public static void AutoField(Rect rect, SerializedProperty property, GUIContent label)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    property.intValue = EditorGUI.IntField(rect, label, property.intValue);
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = EditorGUI.FloatField(rect, label, property.floatValue);
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = EditorGUI.Toggle(rect, label, property.boolValue);
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = EditorGUI.TextField(rect, label, property.stringValue);
                    break;
                case SerializedPropertyType.Color:
                    property.colorValue = EditorGUI.ColorField(rect, label, property.colorValue);
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = EditorGUI.Vector2Field(rect, label, property.vector2Value);
                    break;
                case SerializedPropertyType.Vector3:
                    property.vector3Value = EditorGUI.Vector3Field(rect, label, property.vector3Value);
                    break;
                case SerializedPropertyType.Vector4:
                    property.vector4Value = EditorGUI.Vector4Field(rect, label, property.vector4Value);
                    break;
                case SerializedPropertyType.Vector2Int:
                    property.vector2IntValue = EditorGUI.Vector2IntField(rect, label, property.vector2IntValue);
                    break;
                case SerializedPropertyType.Vector3Int:
                    property.vector3IntValue = EditorGUI.Vector3IntField(rect, label, property.vector3IntValue);
                    break;
                case SerializedPropertyType.Enum:
                    property.intValue = EditorGUI.Popup(rect, label.text, property.enumValueIndex, property.enumDisplayNames);
                    break;
                // TODO:more type
                default:
                    EditorGUI.PropertyField(rect, property, property.isArray);
                    break;
            }
        }
    }
}