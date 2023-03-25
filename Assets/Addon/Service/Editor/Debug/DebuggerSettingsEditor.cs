using System;
using UnityEditor;

namespace Services
{
    [CustomEditor(typeof(DebuggerSettings))]
    public class DebuggerSettingsEditor : Editor
    {
        public SerializedProperty flags;

        private bool foldout;

        private void OnEnable()
        {
            flags = serializedObject.FindProperty(nameof(flags));
            foldout = true;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Fix();
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "允许的消息类型");
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < flags.arraySize; i++)
                {
                    SerializedProperty element = flags.GetArrayElementAtIndex(i);
                    element.boolValue = EditorGUILayout.Toggle(((EMessageType)i).ToString(), element.boolValue);
                }
                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void Fix()
        {
            int length = Enum.GetValues(typeof(EMessageType)).Length;
            int current = flags.arraySize;
            if (length > current)
            {
                for (int i = current; i < length; i++)
                {
                    flags.InsertArrayElementAtIndex(i);
                }
            }
            else
            {
                for (int i = current - 1; i > length - 1; i--)
                {
                    flags.DeleteArrayElementAtIndex(i);
                }
            }
        }
    }
}