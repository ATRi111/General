using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(GridManagerBase),true)]
    public class GridManagerBaseEditor : AutoEditor
    {
        public GridManagerBase GridManager => target as GridManagerBase;

        [AutoProperty]
        public SerializedProperty centerOffset;

        protected override void MyOnInspectorGUI()
        {
            RefreshObjects();
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("物体总数", GridManager.ObjectDict.Count);
            EditorGUI.EndDisabledGroup();
            if (Application.isPlaying)
                return;

            centerOffset.Vector2Field("中心偏移");
            if (GUILayout.Button("全部刷新"))
            {
                GridObject[] gridObjects = GridManager.GetComponentsInChildren<GridObject>();
                for (int i = 0; i < gridObjects.Length; i++)
                {
                    gridObjects[i].Refresh();
                }
            }
            if (GUILayout.Button("全部Z不变对齐"))
            {
                GridObject[] gridObjects = GridManager.GetComponentsInChildren<GridObject>();
                for (int i = 0; i < gridObjects.Length; i++)
                {
                    SerializedObject temp = new(gridObjects[i]);
                    SerializedProperty cellPosition = temp.FindProperty(nameof(cellPosition));
                    cellPosition.vector3IntValue = gridObjects[i].AlignXY();
                    temp.ApplyModifiedProperties();
                }
            }
            if (GUILayout.Button("全部XY不变对齐"))
            {
                GridObject[] gridObjects = GridManager.GetComponentsInChildren<GridObject>();
                for (int i = 0; i < gridObjects.Length; i++)
                {
                    SerializedObject temp = new(gridObjects[i]);
                    SerializedProperty cellPosition = temp.FindProperty(nameof(cellPosition));
                    cellPosition.vector3IntValue = gridObjects[i].AlignZ();
                    temp.ApplyModifiedProperties();
                }
            }
        }

        protected override void MyOnSceneGUI()
        {
            base.MyOnSceneGUI();
            RefreshObjects();
        }

        protected void RefreshObjects()
        {
            if(!Application.isPlaying)
            {
                GridObject[] objects = GridManager.GetComponentsInChildren<GridObject>();
                if (GridManager.ObjectDict.Count != objects.Length)
                    GridManager.AddAllObjects();
            }
        }
    }
}