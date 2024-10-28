using UnityEditor;
using UnityEngine;

namespace AStar.Sample
{
    [CustomEditor(typeof(PathFindingSample))]
    public class PathFindingSampleEditor : Editor
    {
        public SerializedProperty getAdjoinedNodesSO, process, hCostWeight, moveAbility;
        public PathFindingSample sample;

        private void OnEnable()
        {
            sample = target as PathFindingSample;
            getAdjoinedNodesSO = serializedObject.FindProperty(nameof(getAdjoinedNodesSO));
            process = serializedObject.FindProperty(nameof(process));
            hCostWeight = serializedObject.FindProperty(nameof(hCostWeight));
            moveAbility = serializedObject.FindProperty(nameof(moveAbility));
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical();

            EditorGUILayout.PropertyField(getAdjoinedNodesSO, new GUIContent("获取相邻节点的方法"));
            hCostWeight.floatValue = EditorGUILayout.FloatField("hCost权重", hCostWeight.floatValue);
            moveAbility.floatValue = EditorGUILayout.FloatField("移动力", moveAbility.floatValue);
            if (Application.isPlaying)
            {
                if (GUILayout.Button("开始寻路"))
                    sample.StartPathFinding();
                if (GUILayout.Button("下一步"))
                    sample.Next();
                if (GUILayout.Button("立刻完成寻路"))
                    sample.Complete();
                if (GUILayout.Button("清空结果"))
                    sample.Clear();
                EditorGUILayout.PropertyField(process, new GUIContent("寻路过程"));
            }

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}