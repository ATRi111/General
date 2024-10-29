using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(GridObject),true)]
    public class GridObjectEditor : AutoEditor
    {
        public GridObject GridObject => target as GridObject;
        [AutoProperty]
        public SerializedProperty shortName, cellPosition;

        private Vector3Int prev;

        protected override void OnEnable()
        {
            base.OnEnable();
            prev = cellPosition.vector3IntValue;
        }

        protected override void MyOnInspectorGUI()
        {
            shortName.TextField("����(�ɿ�)");
            cellPosition.Vector3IntField("��������");
            if (cellPosition.vector3IntValue != prev)
            {
                prev = cellPosition.vector3IntValue;
                GridObject.CellPosition = cellPosition.vector3IntValue;
            }
            if (GUILayout.Button("Z�������"))
            {
                cellPosition.vector3IntValue = GridObject.AlignXY();
            }
            if (GUILayout.Button("XY�������"))
            {
                cellPosition.vector3IntValue = GridObject.AlignZ();
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("���ô���", GridObject.referenceCount);
            EditorGUI.EndDisabledGroup();
        }
    }
}