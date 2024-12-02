using UnityEngine;

namespace EditorExtend.GridEditor
{
    public static class GridUtility
    {
        public const int MaxHeight = 114514;

        public const float Diagnol = 1.41421356f;
        /// <summary>
        /// (网格坐标系中)一格中心点的坐标相对于该格坐标的偏移量，进行各种求交时均应注意此偏移量
        /// </summary>
        public static Vector3 CenterOffset = 0.5f * Vector3.one;

        public static readonly Vector3Int[] AdjoinPoints8 = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.left + Vector3Int.up,
            Vector3Int.left,
            Vector3Int.left + Vector3Int.down,
            Vector3Int.down,
            Vector3Int.right + Vector3Int.down,
            Vector3Int.right,
            Vector3Int.right + Vector3Int.up,
        };
        public static readonly Vector3Int[] AdjoinPoints4 = new Vector3Int[]
        {
            Vector3Int.up,
            Vector3Int.left,
            Vector3Int.down,
            Vector3Int.right,
        };

        internal static Vector3 ResetZ(this Vector3 v, float z = 0f)
            => new(v.x, v.y, z);
        internal static Vector3Int ResetZ(this Vector3Int v, int z = 0)
            => new(v.x, v.y, z);
        internal static Vector3 AddZ(this Vector2 v, float z)
            => new(v.x, v.y, z);
        internal static Vector3Int AddZ(this Vector2Int v, int z)
           => new(v.x, v.y, z);

        internal static Vector3Int Integerized(this Vector3 v)
            => new(Mathf.RoundToInt(v.x), Mathf.RoundToInt(v.y), Mathf.RoundToInt(v.z));

        public static float ClampAngle(float angle)
        {
            angle %= 360f;
            if (angle < 0f)
                angle += 360f;
            return angle;
        }
        internal static Vector2 ToDirection(this float angle)
            => new(-Mathf.Sin(angle * Mathf.Deg2Rad), Mathf.Cos(angle * Mathf.Deg2Rad));
        internal static float ToAngle(this Vector2 direction)
        {
            float angle = -Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
            return ClampAngle(angle);
        }
    }
}