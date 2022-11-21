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
            EditorWindow editorWindow = GetWindow<DebuggerSettingsEditorWindow>("DebuggerSettings");
            editorWindow.Show();
        }

        private DebuggerSettings settings;
        private bool foldout;

        private void OnEnable()
        {
            settings = Resources.Load<DebuggerSettings>("DebuggerSettings");
            foldout = true;
        }

        private void OnGUI()
        {
            Fix();
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "Flags");
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < settings.flags.Count; i++)
                {
                    settings.flags[i] = EditorGUILayout.Toggle(((EMessageType)i).ToString(), settings.flags[i]);
                }
                EditorGUI.indentLevel--;
            }
        }

        private void Fix()
        {
            int length = Enum.GetValues(typeof(EMessageType)).Length;
            int current = settings.flags.Count;
            if (length > current)
            {
                for (int i = current; i < length; i++)
                {
                    settings.flags.Add(false);
                }
            }
            else
            {
                for (int i = current - 1; i > length - 1; i--)
                {
                    settings.flags.RemoveAt(i);
                }
            }
        }
    }
}