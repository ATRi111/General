using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AStar.ThreeD
{
    public static class PathFinding3DUtility
    {
        public static Node3D GenerateNode_Default(PathFinding3DProcess process, Vector3Int position)
        {
            return new Node3D(process, position);
        }

        #region 六向寻路（面相邻）

        public static readonly ReadOnlyCollection<Vector3Int> SixDirections;

        /// <summary>
        /// 求曼哈顿距离
        /// </summary>
        public static float ManhattanDistance(Vector3Int a, Vector3Int b)
            => Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y) + Mathf.Abs(a.z - b.z);

        /// <summary>
        /// 获取某节点周围面相邻的六个节点
        /// </summary>
        public static void GetAdjoinNodes_Six(PathFinding3DProcess process, Node3D from, Func<Node3D, Node3D, bool> moveCheck, List<Node> ret)
        {
            ret.Clear();
            Node3D to;
            foreach (Vector3Int direction in SixDirections)
            {
                to = process.GetNode(from.Position + direction);
                if (to != null && moveCheck(from, to))
                    ret.Add(to);
            }
        }

        #endregion

        #region 二十六向寻路（面、边、角相邻）

        public static readonly ReadOnlyCollection<Vector3Int> TwentySixDirections;

        /// <summary>
        /// 求对角线距离
        /// </summary>
        public static float DiagonalDistance(Vector3Int a, Vector3Int b)
        {
            float dx = Mathf.Abs(a.x - b.x);
            float dy = Mathf.Abs(a.y - b.y);
            float dz = Mathf.Abs(a.z - b.z);
            float dmax = Mathf.Max(dx, Mathf.Max(dy, dz));
            float dmin = Mathf.Min(dx, Mathf.Min(dy, dz));
            float dmid = dx + dy + dz - dmax - dmin;
            return (dmax - dmid) + dmid * PathFindingUtility.Diagonal2D + dmin * PathFindingUtility.Diagonal3D;
        }

        /// <summary>
        /// 获取某节点周围面、边、角相邻的二十六个节点
        /// </summary>
        public static void GetAdjoinNodes_TwentySix(PathFinding3DProcess process, Node3D from, Func<Node3D, Node3D, bool> moveCheck, List<Node> ret)
        {
            ret.Clear();
            Node3D to;
            foreach (Vector3Int direction in TwentySixDirections)
            {
                to = process.GetNode(from.Position + direction);
                if (to != null && moveCheck(from, to))
                    ret.Add(to);
            }
        }

        #endregion

        static PathFinding3DUtility()
        {
            Vector3Int[] six = new Vector3Int[]
            {
                Vector3Int.up,
                Vector3Int.down,
                Vector3Int.left,
                Vector3Int.right,
                Vector3Int.forward,
                Vector3Int.back,
            };
            SixDirections = new ReadOnlyCollection<Vector3Int>(six);

            List<Vector3Int> twentySix = new();
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        if (x == 0 && y == 0 && z == 0)
                            continue;
                        twentySix.Add(new Vector3Int(x, y, z));
                    }
                }
            }
            TwentySixDirections = new ReadOnlyCollection<Vector3Int>(twentySix);
        }
    }
}
