using UnityEditor;

namespace Services
{
    [CustomEditor(typeof(PerformancePlayerBase),true)]
    public class PerformancePlayerEditor : Editor
    {
        public SerializedProperty useDollFlags;
        public EDollType enumFlagsValue;

        private void OnEnable()
        {
            useDollFlags = serializedObject.FindProperty(nameof(useDollFlags));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            enumFlagsValue = (EDollType)EditorGUILayout.EnumFlagsField("Ê¹ÓÃµÄDoll", (EDollType)enumFlagsValue);
            EditorGUILayout.IntField("int", (int)enumFlagsValue);
            serializedObject.ApplyModifiedProperties();
        }
    }
}