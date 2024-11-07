using System;
using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class IsometricObjectBrush : ObjectBrush
    {
#if UNITY_EDITOR
        public bool lockLayer;
        public int layer;
        [NonSerialized]
        public bool lockXY;

        public override Vector3Int CalculateCellPosition(Vector3 worldPosition)
        {
            IsometricGridManagerBase igm = Manager as IsometricGridManagerBase;
            if(lockXY)
            {
                cellPosition = igm.ClosestZ(cellPosition, worldPosition);
                return cellPosition;
            }

            int tempLayer;
            if (lockLayer)
            {
                tempLayer = layer;
            }
            else if (!Absorb(worldPosition, out tempLayer))
            {
                tempLayer = 0;
            }
            float z = igm.LayerToWorldZ(tempLayer);  //世界坐标的z分量不影响图像位置,但影响转换后cellPosition的z及xy
            worldPosition = worldPosition.ResetZ(z);
            cellPosition = Manager.WorldToCell(worldPosition);
            return cellPosition;
        }

        /// <summary>
        /// 吸附到附近的GridObject
        /// </summary>
        public bool Absorb(Vector3 worldPosition, out int retLayer)
        {
            IsometricGridManagerBase igm = Manager as IsometricGridManagerBase;
            if (!igm.MatchMaxLayer(worldPosition, out retLayer))
            {
                float sumLayer = 0;
                int count = 0;
                for (int i = 0; i < GridUtility.AdjoinPoints8.Length; i++)
                {
                    Vector3 temp = worldPosition + Manager.CellToWorld(GridUtility.AdjoinPoints8[i]);
                    if (igm.MatchMaxLayer(temp, out int tempLayer))
                    {
                        sumLayer += tempLayer;
                        count++;
                    }
                }
                if(count == 0)
                {
                    retLayer = 0;
                    return false;
                }
                retLayer = Mathf.RoundToInt(sumLayer / count);
                return true;
            }
            return true;
        }

        private static readonly Vector3Int[] BottomCellPositions =
        {
            Vector3Int.up,
            Vector3Int.zero,
            Vector3Int.right,
        };
        protected override void OnDrawGizmosSelected()
        {
            base.OnDrawGizmosSelected();
            Vector3[] points;
            if (cellPosition.z > 0)
            {
                Gizmos.color = Color.green;
                points = new Vector3[BottomCellPositions.Length * 2];
                for (int i = 0; i < BottomCellPositions.Length; i++)
                {
                    points[i * 2] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition).ResetZ();
                    points[i * 2 + 1] = Manager.CellToWorld(BottomCellPositions[i] + cellPosition - cellPosition.z * Vector3Int.forward).ResetZ();
                }
                Gizmos.DrawLineList(points);
            }
        }
#endif
    }
}