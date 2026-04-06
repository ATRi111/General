using UnityEngine;

namespace EditorExtend.GridEditor
{
    public class RectangularObjectBrush : ObjectBrush
    {
#if UNITY_EDITOR

        public override Vector3Int CalculateCellPosition(Vector3 worldPosition, Vector3Int lockedPosition)
        {
            return Manager.WorldToCell(worldPosition.ResetZ(transform.position.z));
        }
#endif
    }
}