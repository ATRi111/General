using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AStar
{
    public static class PathFindingUtility
    {
        //边长
        public static float Side = 10f;
        //对角线长
        public static float Diagnol = 14f;

        /// <summary>
        /// 默认的用于计算Hcost的Weight的方法
        /// </summary>
        public static float CalculateWeight_Default(PathFindingProcess _)
        {
            return 1f;
        }
        /// <summary>
        /// 默认的用于计算两节点间能否移动的方法（不考虑是否相邻）
        /// </summary>
        public static bool CheckPassable_Default(PathNode _, PathNode to)
        {
            return to.Type != ENodeType.Obstacle;
        }
        /// <summary>
        /// 默认的用于确定节点类型的方法
        /// </summary>
        public static ENodeType DefineNodeType_Default(Vector2Int _)
        {
            return ENodeType.Blank;
        }

        #region 四向寻路
        /// <summary>
        /// 四个方向的向量的集合
        /// </summary>
        public static readonly ReadOnlyCollection<Vector2Int> fourDirections;
        /// <summary>
        /// 求曼哈顿距离
        /// </summary>
        public static float ManhattanDistance(Vector2Int a, Vector2Int b)
            => Mathf.Abs(a.x - b.x) * Side + Mathf.Abs(a.y - b.y) * Side;

        /// <summary>
        /// 获取某节点周围的四个节点
        /// </summary>
        public static void GetAdjoinNodes_Four(PathFindingProcess process, PathNode node, List<PathNode> ret)
        {
            ret.Clear();
            foreach (Vector2Int direction in fourDirections)
            {
                ret.Add(process.GetNode(node.Position + direction));
            }
        }

        #endregion

        #region 八向寻路

        /// <summary>
        /// 八个方向的向量的集合
        /// </summary>
        public static readonly ReadOnlyCollection<Vector2Int> eightDirections;
        /// <summary>
        /// 求切比雪夫距离
        /// </summary>
        public static float ChebyshevDistance(Vector2Int a, Vector2Int b)
        {
            float deltaX = Mathf.Abs(a.x - b.x);
            float deltaY = Mathf.Abs(a.y - b.y);
            float max = Mathf.Max(deltaX, deltaY);
            float min = Mathf.Min(deltaX, deltaY);
            return min * Diagnol + (max - min) * Side;
        }
        /// <summary>
        /// 获取某节点周围的八个节点
        /// </summary>
        public static void GetAdjoinNodes_Eight(PathFindingProcess process, PathNode node, List<PathNode> ret)
        {
            ret.Clear();
            foreach (Vector2Int direction in eightDirections)
            {
                ret.Add(process.GetNode(node.Position + direction));
            }
        }
        #endregion

        static PathFindingUtility()
        {
            Vector2Int[] eight = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.left + Vector2Int.up,
                Vector2Int.left,
                Vector2Int.left + Vector2Int.down,
                Vector2Int.down,
                Vector2Int.right + Vector2Int.down,
                Vector2Int.right,
                Vector2Int.right + Vector2Int.up,
            };
            eightDirections = new ReadOnlyCollection<Vector2Int>(eight);
            Vector2Int[] four = new Vector2Int[]
            {
                 Vector2Int.up,
                Vector2Int.left,
                Vector2Int.down,
                Vector2Int.right,
            };
            fourDirections = new ReadOnlyCollection<Vector2Int>(four);
        }
    }
}