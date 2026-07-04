using UnityEngine;

namespace AStar.SVO
{
    public static class PathFindingSVOUtility
    {
        public const float Diagonal2D = 1.41421356f; // √2
        public const float Diagonal3D = 1.73205081f; // √3
        public const float Epsilon = 1e-6f;

        /// <summary>
        /// 曼哈顿距离
        /// </summary>
        public static float ManhattanDistance(Vector3 a, Vector3 b)
            => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);

        /// <summary>
        /// 欧几里得距离
        /// </summary>
        public static float EuclidDistance(Vector3 a, Vector3 b)
            => Vector3.Distance(a, b);

        /// <summary>
        /// 对角线距离
        /// </summary>
        public static float DiagonalDistance(Vector3 a, Vector3 b)
        {
            float dx = Mathf.Abs(a.x - b.x);
            float dy = Mathf.Abs(a.y - b.y);
            float dz = Mathf.Abs(a.z - b.z);
            float dmax = Mathf.Max(dx, Mathf.Max(dy, dz));
            float dmin = Mathf.Min(dx, Mathf.Min(dy, dz));
            float dmid = dx + dy + dz - dmax - dmin;
            return (dmax - dmid) + dmid * Diagonal2D + dmin * Diagonal3D;
        }
    }
}
