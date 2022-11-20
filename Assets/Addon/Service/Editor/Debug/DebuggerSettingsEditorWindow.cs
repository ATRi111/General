using UnityEditor;
using UnityEngine;

namespace Services
{
    public class DebuggerSettingsEditorWindow : EditorWindow
    {
        [MenuItem("Tools/Debug/DebuggerSettings")]
        public static void Open()
        {
            EditorWindow editorWindow = GetWindow<DebuggerSettingsEditorWindow>();
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
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "Flags");
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < settings.flags.Length; i++)
                {
                    settings.flags[i] = EditorGUILayout.Toggle(((EMessageType)i).ToString(), settings.flags[i]);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}