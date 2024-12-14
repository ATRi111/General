using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar.Sample
{
    [CustomEditor(typeof(PathFindingSample))]
    public class PathFindingSampleEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty prefab, process, moveAbility;
        public PathFindingSample sample;

        protected override void OnEnable()
        {
            base.OnEnable();
            sample = target as PathFindingSample;
        }

        protected override void MyOnInspectorGUI()
        {
            prefab.PropertyField("预制体");
            moveAbility.IntField("移动力");
            process.PropertyField("寻路过程");
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
            }
        }
    }
}