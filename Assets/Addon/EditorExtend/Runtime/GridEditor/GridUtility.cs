using UnityEngine;

namespace EditorExtend.GridEditor
{
    public static class GridUtility
    {
        public const int MaxHeight = 114514;

        public const float Diagnol = 1.41421356f;

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

        public static bool BoxOverlap(Vector3 min, Vector3 extend, Vector3 p)
        {
            return p.x >= min.x && p.x < min.x + extend.x
                && p.y >= min.y && p.y < min.y + extend.y
                && p.z >= min.z && p.z < min.z + extend.z;
        }
        public static bool SphereOverlap(Vector3 center, float radius, Vector3 p)
        {
            return (p - center).sqrMagnitude < radius * radius;
        }
        public static bool CylinderOverlap(Vector3 bottomCenter, float height, float radius, Vector3 p)
        {
            if (p.z < bottomCenter.z || p.z >= bottomCenter.z + height) 
                return false;
            float projSqrDistance = (p - bottomCenter).ResetZ().sqrMagnitude;
            return projSqrDistance < radius * radius;
        }
    }
}