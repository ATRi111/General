using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(ObjectBrush),true)]
    public class ObjectBrushEditor : InteractiveEditor
    {
        public ObjectBrush ObjectBrush => target as ObjectBrush;

        private string[] displayOptions;
        [AutoProperty]
        public SerializedProperty prefab, mountIndex;
        private string prefabName;

        protected override void OnEnable()
        {
            base.OnEnable();
            editorModeOnly = true;
            ObjectBrush.MountPoints = null;
            int n = ObjectBrush.MountPoints.Count;
            displayOptions = new string[n];
            prefabName = string.Empty;
            for (int i = 0; i < n; i++)
            {
                displayOptions[i] = ObjectBrush.MountPoints[i].gameObject.name;
            }
        }

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            if (Application.isPlaying)
                return;

            prefab.PropertyField("笔刷");
            if (prefabName != prefab.objectReferenceValue.name)
            {
                prefabName = prefab.objectReferenceValue.name;
                UpdateMountPoint(prefabName);
            }
            mountIndex.intValue = EditorGUILayout.Popup("挂载点", mountIndex.intValue, displayOptions);
        }

        protected void UpdateMountPoint(string prefabName)
        {
            Transform[] transforms = ObjectBrush.GetComponentsInChildren<Transform>();
            for (int i = 0; i < transforms.Length; i++)
            {
                if (transforms[i].gameObject.name == prefabName)
                {
                    for (int j = 0; j < displayOptions.Length; j++)
                    {
                        if (transforms[i].parent.name == displayOptions[j])
                        {
                            mountIndex.intValue = j;
                            return;
                        }
                    }
                }
            }
        }

        protected override void MyOnSceneGUI()
        {
            base.MyOnSceneGUI();
            if (Application.isPlaying)
                return;

            HandleKeyInput();
            UpdateCellPosition();
            HandleMouseInput();
        }

        protected override void HandleMouseInput()
        {
            UpdateCellPosition();
            base.HandleMouseInput();
        }

        protected override void OnMouseDown(int button)
        {
            base.OnMouseDown(button);
            switch(button)
            {
                case 0:
                    Brush();
                    break;
                case 1:
                    Erase();
                    break;
            }
        }
        protected override void OnMouseDrag(int button)
        {
            base.OnMouseDrag(button);
            switch (button)
            {
                case 0:
                    Brush();
                    break;
                case 1:
                    Erase();
                    break;
            }
        }

        protected virtual void UpdateCellPosition()
        {
            Vector3 world = SceneViewUtility.SceneToWorld(mousePosition);
            ObjectBrush.cellPosition = ObjectBrush.CalculateCellPosition(world);
            SceneView.RepaintAll();
        }

        protected virtual void Brush()
        {
            currentEvent.Use();
            if (!ObjectBrush.Manager.CanPlaceAt(ObjectBrush.cellPosition))
                return;

            if (ObjectBrush.prefab != null)
            {
                GameObject obj = PrefabUtility.InstantiatePrefab(ObjectBrush.prefab, ObjectBrush.MountPoint) as GameObject;
                Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
                GridObject gridObject = obj.GetComponent<GridObject>();

                SerializedObject temp = new(gridObject);
                SerializedProperty cellPosition = temp.FindProperty(nameof(cellPosition));
                cellPosition.vector3IntValue = ObjectBrush.cellPosition;
                gridObject.CellPosition = ObjectBrush.cellPosition;
                temp.ApplyModifiedProperties();
                //ObjectBrush.Manager.AddObject(gridObject);   //Editor模式下GridManager会自动刷新以获取新的GridObject
            }
        }

        protected virtual void Erase()
        {
            if(ObjectBrush.Manager.ObjectDict.ContainsKey(ObjectBrush.cellPosition))
            {
                GridObject gridObject = ObjectBrush.Manager.TryRemoveObject(ObjectBrush.cellPosition);
                ExternalTool.Destroy(gridObject.gameObject);
            }
            currentEvent.Use();
        }
    }
}