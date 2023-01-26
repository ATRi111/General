using System;
using UnityEditor;

namespace Services.Performance
{
    [CustomEditor(typeof(PerformancePlayerBase), true)]
    public class PerformancePlayerEditor : Editor
    {
        public SerializedProperty useDolls;
        public bool foldout;

        private void OnEnable()
        {
            useDolls = serializedObject.FindProperty(nameof(useDolls));
            foldout = true;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            Fix();
            foldout = EditorGUILayout.BeginFoldoutHeaderGroup(foldout, "演出中使用的Doll");
            EditorGUILayout.EndFoldoutHeaderGroup();
            if (foldout)
            {
                EditorGUI.indentLevel++;
                for (int i = 0; i < useDolls.arraySize; i++)
                {
                    SerializedProperty element = useDolls.GetArrayElementAtIndex(i);
                    element.boolValue = EditorGUILayout.Toggle(((EDollType)i).ToString(), element.boolValue);
                }
                EditorGUI.indentLevel--;
            }
            serializedObject.ApplyModifiedProperties();
        }

        private void Fix()
        {
            int length = Enum.GetValues(typeof(EDollType)).Length;
            int current = useDolls.arraySize;
            if (length > current)
            {
                for (int i = current; i < length; i++)
                {
                    useDolls.InsertArrayElementAtIndex(i);
                }
            }
            else
            {
                for (int i = current - 1; i > length - 1; i--)
                {
                    useDolls.DeleteArrayElementAtIndex(i);
                }
            }
        }
    }
}