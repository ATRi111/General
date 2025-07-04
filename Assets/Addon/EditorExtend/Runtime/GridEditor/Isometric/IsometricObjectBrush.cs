using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class IsometricObjectBrush : ObjectBrush
    {
#if UNITY_EDITOR
        public bool pillarMode;
        public bool lockLayer;
        public int layer;
        public bool lockXY;

        public override Vector3Int CalculateCellPosition(Vector3 worldPosition, Vector3Int lockedPosition)
        {
            worldPosition = worldPosition.ResetZ(0f);
            IsometricGridManagerBase igm = Manager as IsometricGridManagerBase;
            if (lockXY)
            {
                Vector3Int temp = igm.ClosestZ(lockedPosition.ResetZ(cellPosition.z), worldPosition);  //锁定后，X和Y不能变化
                return temp;
            }

            if (lockLayer)
            {
                float z = igm.LayerToWorldZ(layer);
                Vector3Int temp = Manager.WorldToCell(worldPosition.ResetZ(z));
                //int deltaX = Mathf.Abs(temp.x - lockedPosition.x);
                //int deltaY = Mathf.Abs(temp.y - lockedPosition.y);
                //if(deltaX > 0 && deltaY != 0)       //锁定后，X和Y只能有一个变化
                //{
                //    if(deltaX >= deltaY)
                //        temp.y = lockedPosition.y;
                //    else
                //        temp.x = lockedPosition.x;
                //}
                return temp;
            }

            return igm.AbsorbSurfaceOfCube(worldPosition);
        }
#endif
    }
}