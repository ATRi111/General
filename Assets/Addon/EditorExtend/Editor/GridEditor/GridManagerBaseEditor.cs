using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(GridManagerBase), true)]
    public class GridManagerBaseEditor : AutoEditor
    {
        public GridManagerBase GridManager => target as GridManagerBase;

        [AutoProperty]
        public SerializedProperty centerOffset;

        protected override void MyOnInspectorGUI()
        {
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.IntField("��������", GridManager.ObjectDict.Count);
            EditorGUI.EndDisabledGroup();
            if (Application.isPlaying)
                return;

            centerOffset.Vector2Field("����ƫ��");
            if (GUILayout.Button("ȫ��ˢ��"))
            {
                GridObject[] gridObjects = GridManager.GetComponentsInChildren<GridObject>();
                for (int i = 0; i < gridObjects.Length; i++)
                {
                    gridObjects[i].Refresh();
                }
                GridManager.AddAllObjects();
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            if (GUILayout.Button("ȫ��Z�������"))
            {
                GridObject[] gridObjects = GridManager.GetComponentsInChildren<GridObject>();
                for (int i = 0; i < gridObjects.Length; i++)
                {
                    SerializedObject temp = new(gridObjects[i]);
                    SerializedProperty cellPosition = temp.FindProperty(nameof(cellPosition));
                    cellPosition.vector3IntValue = gridObjects[i].AlignXY();
                    temp.ApplyModifiedProperties();
                }
                GridManager.AddAllObjects();
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
            if (GUILayout.Button("ȫ��XY�������"))
            {
                GridObject[] gridObjects = GridManager.GetComponentsInChildren<GridObject>();
                for (int i = 0; i < gridObjects.Length; i++)
                {
                    SerializedObject temp = new(gridObjects[i]);
                    SerializedProperty cellPosition = temp.FindProperty(nameof(cellPosition));
                    cellPosition.vector3IntValue = gridObjects[i].AlignZ();
                    temp.ApplyModifiedProperties();
                }
                GridManager.AddAllObjects();
                if (!Application.isPlaying)
                    EditorSceneManager.MarkSceneDirty(SceneManager.GetActiveScene());
            }
        }
    }
}