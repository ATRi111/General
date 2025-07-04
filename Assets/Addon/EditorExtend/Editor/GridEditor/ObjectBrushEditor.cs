using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(ObjectBrush), true)]
    public class ObjectBrushEditor : InteractiveEditor
    {
        public ObjectBrush ObjectBrush => target as ObjectBrush;
        protected GridManagerBase manager;

        private string[] displayOptions;
        [AutoProperty]
        public SerializedProperty prefab, mountIndex, overrideMode;
        private string prefabName;

        protected bool isEditting;
        protected Vector3Int lockedPosition;

        protected List<Vector3> editorPoints;

        protected override void OnEnable()
        {
            base.OnEnable();
            editorModeOnly = true;
            ObjectBrush.MountPoints = null;
            manager = ObjectBrush.GetComponent<GridManagerBase>();
            int n = ObjectBrush.MountPoints.Count;
            displayOptions = new string[n];
            prefabName = string.Empty;
            for (int i = 0; i < n; i++)
            {
                displayOptions[i] = ObjectBrush.MountPoints[i].gameObject.name;
            }
            editorPoints = new();
            isEditting = false;
        }

        //确保聚焦到Scene窗口
        public void Focus()
        {
            if (SceneView.currentDrawingSceneView == null && SceneView.lastActiveSceneView != null)
            {
                SceneView.lastActiveSceneView.Focus();
                SceneView.RepaintAll();
            }
        }

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            if (Application.isPlaying)
                return;

            if (isEditting)
            {
                if (GUILayout.Button("停止编辑"))
                {
                    isEditting = false;
                }

                prefab.PropertyField("笔刷");
                if (prefab.objectReferenceValue != null && prefabName != prefab.objectReferenceValue.name)
                {
                    prefabName = prefab.objectReferenceValue.name;
                    UpdateMountPoint(prefabName);
                }
                mountIndex.intValue = EditorGUILayout.Popup("挂载点", mountIndex.intValue, displayOptions);
                overrideMode.BoolField("强制覆盖模式");

            }
            else
            {
                if (GUILayout.Button("开始编辑"))
                {
                    isEditting = true;
                    Focus();
                }
            }
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
            if (Application.isPlaying || !isEditting)
                return;

            HandleKeyInput();
            UpdateCellPosition();
            HandleMouseInput();
            if (currentEvent.type == EventType.Repaint)
                Paint();
        }

        protected override void Paint()
        {
            base.Paint();
            Handles.color = Color.red;
            GameObject obj = prefab.objectReferenceValue as GameObject;
            if (obj == null)
            {
                IGridShape.GetStrip_Default(editorPoints);
            }
            else
            {
                GridCollider collider = obj.GetComponentInChildren<GridCollider>();
                if (collider == null)
                    IGridShape.GetStrip_Default(editorPoints);
                else
                    collider.GetStrip(editorPoints);
            }
            Vector3[] temp = new Vector3[editorPoints.Count];
            for (int i = 0; i < editorPoints.Count; i++)
            {
                temp[i] = manager.CellToWorld(editorPoints[i] + ObjectBrush.cellPosition);
                temp[i] = new Vector3(temp[i].x, temp[i].y);
            }
            HandleUI.DrawLineStrip(temp, 1f, false);
        }

        protected override void HandleMouseInput()
        {
            UpdateCellPosition();
            base.HandleMouseInput();
        }

        protected override void OnMouseDown(int button)
        {
            base.OnMouseDown(button);
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
            Vector3Int temp = ObjectBrush.CalculateCellPosition(world, lockedPosition);
            if (ObjectBrush.cellPosition != temp)
            {
                ObjectBrush.cellPosition = temp;
                Paint();
                SceneView.RepaintAll();
            }
        }

        protected virtual GridObject TryBrushAt(Vector3Int position)
        {
            if (ObjectBrush.prefab == null)
                return null;

            if (!ObjectBrush.Manager.CanPlaceAt(position))
            {
                if (overrideMode.boolValue)
                    TryEraseAt(position);
                else
                    return null;
            }

            GameObject obj = PrefabUtility.InstantiatePrefab(ObjectBrush.prefab, ObjectBrush.MountPoint) as GameObject;
            Undo.RegisterCreatedObjectUndo(obj, "Create " + obj.name);
            GridObject gridObject = obj.GetComponent<GridObject>();
            gridObject.CellPosition = position;

            return gridObject;
        }

        protected virtual void Brush()
        {
            currentEvent.Use();
            TryBrushAt(ObjectBrush.cellPosition);
        }

        protected virtual bool TryEraseAt(Vector3Int position)
        {
            if (ObjectBrush.Manager.ObjectDict.ContainsKey(position))
            {
                GridObject gridObject = ObjectBrush.Manager.TryRemoveObject(position);
                if (gridObject != null)
                {
                    ExternalTool.Destroy(gridObject.gameObject);
                    return true;
                }
            }
            return false;
        }

        protected virtual void Erase()
        {
            currentEvent.Use();
            TryEraseAt(ObjectBrush.cellPosition);
        }
    }
}