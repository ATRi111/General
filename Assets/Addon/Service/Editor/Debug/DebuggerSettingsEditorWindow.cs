using System;
using UnityEditor;
using UnityEngine;

namespace Services
{
    public class DebuggerSettingsEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Debug/DebuggerSettings")]
        public static void Open()
        {
            EditorWindow editorWindow = GetWindow<DebuggerSettingsEditorWindow>("SetDebug");
            editorWindow.Show();
        }

        private DebuggerSettingSO so;
        private bool foldout;

        private void OnEnable()
        {
            so = Resources.Load<DebuggerSettingSO>("DebuggerSettings");
            foldout = true;
            minSize = new Vector2(200, 200);
            maxSize = new Vector2(300, 500);
        }

        private void OnGUI()
        {
            Fix();
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "允许的消息类型");
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < so.settings.flags.Count; i++)
                {
                    so.settings.flags[i] = EditorGUILayout.Toggle(((EMessageType)i).ToString(), so.settings.flags[i]);
                }
                EditorGUI.indentLevel--;
            }
            EditorUtility.SetDirty(so);
        }

        private void Fix()
        {
            int length = Enum.GetValues(typeof(EMessageType)).Length;
            int current = so.settings.flags.Count;
            if (length > current)
            {
                for (int i = current; i < length; i++)
                {
                    so.settings.flags.Add(false);
                }
            }
            else
            {
                for (int i = current - 1; i > length - 1; i--)
                {
                    so.settings.flags.RemoveAt(i);
                }
            }
        }
    }
}