using UnityEngine;

namespace EditorExtend.GridEditor
{
    public static class IsometricGridUtility
    {
        public static Vector3 CellToWorld(Vector3 cellPosition, Vector3 cellSize)
        {
            float x = 0.5f * cellPosition.x * cellSize.x - 0.5f * cellPosition.y * cellSize.x;
            float y = 0.5f * cellPosition.x * cellSize.y + 0.5f * cellPosition.y * cellSize.y + 0.5f * cellPosition.z * cellSize.y * cellSize.z;
            float z = cellPosition.z * cellSize.z;
            return new(x, y, z);
        }
    }
}