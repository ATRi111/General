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
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "Flags");
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < flags.arraySize; i++)
                {
                    SerializedProperty element = flags.GetArrayElementAtIndex(i);
                    EditorGUILayout.Toggle(((EMessageType)i).ToString(), element.boolValue);
                }
                EditorGUI.indentLevel--;
            }
        }
    }
}