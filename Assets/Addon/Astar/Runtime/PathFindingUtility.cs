using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AStar
{
    public static class PathFindingUtility
    {
        public const float Diagonal = 1.41421356f;
        public const float Epsilon = 1e-6f;

        public static float CalculateWeight_Default(PathFindingProcess _)
        {
            return 1f;
        }

        public static bool CheckObstacle_Default()
        {
            return true;
        }
        public static Node GenerateNode_Default(PathFindingProcess process, Vector2Int position)
        {
            return new Node(process, position);
        }

        #region 四向寻路

        public static readonly ReadOnlyCollection<Vector2Int> FourDirections;
        /// <summary>
        /// 求曼哈顿距离
        /// </summary>
        public static float ManhattanDistance(Vector2Int a, Vector2Int b)
            => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);

        /// <summary>
        /// 获取某节点周围的四个节点
        /// </summary>
        public static void GetAdjoinNodes_Four(PathFindingProcess process, Node node, List<Node> ret)
        {
            ret.Clear();
            foreach (Vector2Int direction in FourDirections)
            {
                ret.Add(process.GetNode(node.Position + direction));
            }
        }

        #endregion

        #region 八向寻路

        public static readonly ReadOnlyCollection<Vector2Int> EightDirections;

        /// <summary>
        /// 求切比雪夫距离
        /// </summary>
        public static float ChebyshevDistance(Vector2Int a, Vector2Int b)
        {
            float deltaX = Mathf.Abs(a.x - b.x);
            float deltaY = Mathf.Abs(a.y - b.y);
            float max = Mathf.Max(deltaX, deltaY);
            float min = Mathf.Min(deltaX, deltaY);
            return min * Diagonal + (max - min);
        }
        /// <summary>
        /// 获取某节点周围的八个节点
        /// </summary>
        public static void GetAdjoinNodes_Eight(PathFindingProcess process, Node node, List<Node> ret)
        {
            ret.Clear();
            foreach (Vector2Int direction in EightDirections)
            {
                ret.Add(process.GetNode(node.Position + direction));
            }
        }
        #endregion

        /// <summary>
        /// 对向量按与某个向量的夹角大小排序;
        /// 两侧存在到当前向量夹角大小相等的一对向量时，能确保总是返回某一侧的向量
        /// </summary>
        public class Comparer_Vector2_Nearer : IComparer<Vector2>, IComparer<Vector2Int>
        {
            public Vector2 direction;

            public Comparer_Vector2_Nearer(Vector2 direction)
            {
                this.direction = direction;
            }

            public int Compare(Vector2 x, Vector2 y)
            {
                float angleX = Vector2.Angle(direction, x);
                float angleY = Vector2.Angle(direction, y);
                if (angleX != angleY)
                    return angleX.CompareTo(angleY);
                float zX = Vector3.Cross(direction, x).z;
                float zY = Vector3.Cross(direction, y).z;
                return zX.CompareTo(zY);
            }

            public int Compare(Vector2Int x, Vector2Int y)
            {
                return Compare((Vector2)x, (Vector2)y);
            }
        }

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
            EightDirections = new ReadOnlyCollection<Vector2Int>(eight);
            Vector2Int[] four = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.left,
                Vector2Int.down,
                Vector2Int.right,
            };
            FourDirections = new ReadOnlyCollection<Vector2Int>(four);
        }
    }
}