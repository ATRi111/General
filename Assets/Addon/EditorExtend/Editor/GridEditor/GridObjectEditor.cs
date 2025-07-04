using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(GridObject), true)]
    public class GridObjectEditor : AutoEditor
    {
        public GridObject GridObject => target as GridObject;
        [AutoProperty]
        public SerializedProperty cellPosition, groundHeight;

        private Vector3Int prev;

        protected override void OnEnable()
        {
            base.OnEnable();
            if (GridObject.Manager != null)
                prev = cellPosition.vector3IntValue = GridObject.Manager.ClosestCell(GridObject.transform.position);
            serializedObject.ApplyModifiedProperties();
        }

        protected override void MyOnInspectorGUI()
        {
            cellPosition.Vector3IntField("��������");
            groundHeight.IntField("����߶�");
            if (cellPosition.vector3IntValue != prev)
            {
                prev = cellPosition.vector3IntValue;
                GridObject.CellPosition = cellPosition.vector3IntValue;
            }
            if (GUILayout.Button("Z�������"))
            {
                cellPosition.vector3IntValue = GridObject.AlignXY();
                EditorUtility.SetDirty(target);
            }
            if (GUILayout.Button("XY�������"))
            {
                cellPosition.vector3IntValue = GridObject.AlignZ();
                EditorUtility.SetDirty(target);
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("���ô���", GridObject.referenceCount);
            EditorGUI.EndDisabledGroup();
        }
    }
}