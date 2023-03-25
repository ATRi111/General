using UnityEditor;
using UnityEngine;

namespace Services.Save
{
    [CustomEditor(typeof(SaveManagerBase), true)]
    public class SaveManagerBaseEditor : Editor
    {
        public SerializedProperty core;
        public SerializedProperty runtimeData;

        private void OnEnable()
        {
            core = serializedObject.FindProperty(nameof(core));
            runtimeData = core.FindPropertyRelative(nameof(runtimeData));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.PropertyField(runtimeData, new GUIContent("运行时数据"));
            serializedObject.ApplyModifiedProperties();
        }
    }
}