using EditorExtend;
using System;
using UnityEditor;

namespace Services
{
    [CustomEditor(typeof(DebuggerSettings))]
    public class DebuggerSettingsEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty flags;

        private bool foldout;

        protected override void OnEnable()
        {
            base.OnEnable();
            Fix();
            foldout = true;
        }

        protected override void MyOnInspectorGUI()
        {

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