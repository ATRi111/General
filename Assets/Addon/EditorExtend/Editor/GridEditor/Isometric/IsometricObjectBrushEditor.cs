using MyTool;
using UnityEditor;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    [CustomEditor(typeof(IsometricObjectBrush))]
    public class IsometricObjectBrushEditor : ObjectBrushEditor
    {
        private static readonly Vector3Int[] BottomCellPositions =
        {
            Vector3Int.up,
            Vector3Int.zero,
            Vector3Int.right,
        };

        public new IsometricObjectBrush ObjectBrush => target as IsometricObjectBrush;
        [AutoProperty]
        public SerializedProperty pillarMode, lockLayer, layer, lockXY;

        protected override void OnEnable()
        {
            base.OnEnable();
            lockXY.boolValue = false;
            lockLayer.boolValue = false;
        }

        protected override void MyOnInspectorGUI()
        {
            base.MyOnInspectorGUI();
            if (isEditting)
            {
                pillarMode.BoolField("柱形绘制模式");
                lockXY.BoolField("锁定XY");
                lockLayer.BoolField("锁定层数");
                if (lockLayer.boolValue)
                {
                    layer.IntField("层数");
                }
                EditorGUILayout.HelpBox("按住Ctrl锁定XY;按住Shift锁定层数", MessageType.Info);
            }
        }

        protected override void Paint()
        {
            base.Paint();
            Vector3Int cellPosition = ObjectBrush.cellPosition;
            if (cellPosition.z > 0)
            {
                Handles.color = Color.green;
                Gizmos.color = Color.green;
                for (int i = 0; i < BottomCellPositions.Length; i++)
                {
                    Vector3 from = manager.CellToWorld(BottomCellPositions[i] + cellPosition).ResetZ();
                    Vector3 to = manager.CellToWorld(BottomCellPositions[i] + cellPosition - cellPosition.z * Vector3Int.forward).ResetZ();
                    HandleUI.DrawLine(from, to, 1f);
                }
            }
        }

        protected override void Brush()
        {
            base.Brush();
            if(ObjectBrush.pillarMode)
            {
                GridObject gridObject = ObjectBrush.prefab.GetComponent<GridObject>();
                if (gridObject.IsGround)
                {
                    Vector3Int position = ObjectBrush.cellPosition + Vector3Int.back;
                    for (; position.z >= 0; position += Vector3Int.back)
                    {
                        TryBrushAt(position);
                    }
                }
            }
        }

        protected override GridObject TryBrushAt(Vector3Int position)
        {
            GridObject gridObject = base.TryBrushAt(position);
            GridSortingOrderControllerBase[] controllers = gridObject.GetComponentsInChildren<GridSortingOrderControllerBase>();
            for (int i = 0; i < controllers.Length; i++)
            {
                controllers[i].RefreshSortingOrder();
            }
            return gridObject;
        }

        protected override void Erase()
        {
            base.Erase(); 
            if (ObjectBrush.pillarMode && lockLayer.boolValue)
            {
                GridObject gridObject = ObjectBrush.prefab.GetComponent<GridObject>();
                if (gridObject.IsGround)
                {
                    Vector3Int position = ObjectBrush.cellPosition + Vector3Int.back;
                    for (; position.z >= 0; position += Vector3Int.back)
                    {
                        TryEraseAt(position);
                    }
                }
            }
        }

        protected override void OnKeyDown(KeyCode keyCode)
        {
            if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
            {
                currentEvent.Use();
                UpdateCellPosition();
                lockXY.boolValue = true;
                lockLayer.boolValue = false;
                lockedPosition = ObjectBrush.cellPosition;
            }
            else if (keyCode == KeyCode.LeftShift || keyCode == KeyCode.RightShift)
            {
                currentEvent.Use();
                UpdateCellPosition();
                lockLayer.boolValue = true;
                layer.intValue = ObjectBrush.cellPosition.z;
                lockXY.boolValue = false;
                lockedPosition = ObjectBrush.cellPosition;
            }
        }
        protected override void OnKeyUp(KeyCode keyCode)
        {
            if (keyCode == KeyCode.LeftControl || keyCode == KeyCode.RightControl)
            {
                currentEvent.Use();
                lockXY.boolValue = false;
            }
            else if (keyCode == KeyCode.LeftShift || keyCode == KeyCode.RightShift)
            {
                currentEvent.Use();
                lockLayer.boolValue = false;
            }
        }
    }
}