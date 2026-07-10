using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace AStar.Sample
{
    [CustomEditor(typeof(PathFindingSample3D))]
    public class PathFindingSample3DEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty prefab, process, moveAbility, verticalBuffer, hideNodesOutsidePath;
        public PathFindingSample3D sample;

        protected override void OnEnable()
        {
            base.OnEnable();
            sample = target as PathFindingSample3D;
        }

        protected override void MyOnInspectorGUI()
        {
            prefab.PropertyField("预制体");
            moveAbility.IntField("移动力");
            verticalBuffer.IntField("竖直方向障碍物顶部预留空间");

            EditorGUI.BeginChangeCheck();
            hideNodesOutsidePath.BoolField("隐藏路径以外的节点");
            if (EditorGUI.EndChangeCheck())
            {
                // 勾选框改变的效果只体现在Repaint画出来的调试物体上，不改变process数据本身；
                // 提前ApplyModifiedProperties让sample.hideNodesOutsidePath拿到最新值，再立刻重绘一次，
                // 不用等到下一次点"下一步"/"立刻完成寻路"才看到效果
                serializedObject.ApplyModifiedProperties();
                if (Application.isPlaying)
                    sample.Repaint();
            }

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
