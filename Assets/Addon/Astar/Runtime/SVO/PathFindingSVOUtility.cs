using UnityEngine;

namespace AStar.SVO
{
    /// <summary>
    /// 稀疏体素八叉树寻路使用的距离计算方法
    /// </summary>
    public static class PathFindingSVOUtility
    {
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
            return (dmax - dmid) + dmid * AStar.PathFindingUtility.Diagonal2D + dmin * AStar.PathFindingUtility.Diagonal3D;
        }
    }
}
