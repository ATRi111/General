using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

namespace AStar.TwoD
{
    public static class PathFinding2DUtility
    {
        public static Node2D GenerateNode_Default(PathFinding2DProcess process, Vector2Int position)
        {
            return new Node2D(process, position);
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
        public static void GetAdjoin4Nodes(PathFinding2DProcess process, Node2D from, Func<Node2D, Node2D, bool> moveCheck, List<Node> ret)
        {
            ret.Clear();
            Node2D to;
            foreach (Vector2Int direction in FourDirections)
            {
                to = process.GetNode(from.Position + direction);
                if (to != null && moveCheck(from, to))
                    ret.Add(to);
            }
        }

        #endregion

        #region 八向寻路

        public static readonly ReadOnlyCollection<Vector2Int> EightDirections;
        public static readonly ReadOnlyCollection<Vector2Int> DiagnolDirections;
        public static readonly Dictionary<Vector2Int, Vector2Int[]> SortedEightDirections;

        /// <summary>
        /// 求对角线距离
        /// </summary>
        public static float DiagonalDistance(Vector2Int a, Vector2Int b)
        {
            float deltaX = Mathf.Abs(a.x - b.x);
            float deltaY = Mathf.Abs(a.y - b.y);
            float max = Mathf.Max(deltaX, deltaY);
            float min = Mathf.Min(deltaX, deltaY);
            return min * PathFindingUtility.Diagonal2D + (max - min);
        }
        /// <summary>
        /// 获取某节点周围的八个节点
        /// </summary>
        public static void GetAdjoin8Nodes(PathFinding2DProcess process, Node2D from, Func<Node2D, Node2D, bool> moveCheck, List<Node> ret)
        {
            bool CantPass(Node2D to)
            {
                if (to == null)
                    return true;
                return !moveCheck(from, to);
            }

            ret.Clear();
            Node2D to;
            foreach (Vector2Int direction in FourDirections)
            {
                to = process.GetNode(from.Position + direction);
                if (CantPass(to))
                    continue;
                ret.Add(to);
            }

            foreach (Vector2Int direction in DiagnolDirections)
            {
                to = process.GetNode(from.Position + direction);
                if (CantPass(to))
                    continue;
                to = process.GetNode(from.Position + new Vector2Int(direction.x, 0));
                if (CantPass(to))
                    continue;
                to = process.GetNode(from.Position + new Vector2Int(0, direction.y));
                if (CantPass(to))
                    continue;
                ret.Add(to);
            }
        }
        #endregion

        /// <summary>
        /// 判断两个方向是否相同（至少有一个0向量时返回false）
        /// </summary>
        public static bool Align(Vector2Int a, Vector2Int b)
        {
            int cross = a.x * b.y - a.y * b.x;
            if(cross != 0)
                return false;
            int dot = a.x * b.x + a.y * b.y;
            return dot > 0;
        }

        /// <summary>
        /// 判断一个方向是否与4个方向之一相同
        /// </summary>
        public static bool Align4(Vector2Int v)
        {
            foreach (Vector2Int direction in FourDirections)
            {
                if (Align(v, direction))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断一个方向是否与8个方向之一相同
        /// </summary>
        public static bool Align8(Vector2Int v)
        {
            foreach (Vector2Int direction in EightDirections)
            {
                if (Align(v, direction))
                    return true;
            }
            return false;
        }

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

        static PathFinding2DUtility()
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

            Vector2Int[] diag = new Vector2Int[]
            {
                Vector2Int.left + Vector2Int.up,
                Vector2Int.left + Vector2Int.down,
                Vector2Int.right + Vector2Int.down,
                Vector2Int.right + Vector2Int.up,
            };
            DiagnolDirections = new ReadOnlyCollection<Vector2Int>(diag);

            SortedEightDirections = new();
            foreach (Vector2Int direction in EightDirections)
            {
                Vector2Int[] directions = EightDirections.ToArray();
                Comparer_Vector2_Nearer comparer = new(direction);
                Array.Sort(directions, comparer);
                SortedEightDirections.Add(direction, directions);
            }
        }
    }
}
