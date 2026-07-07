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
        public static void Get6AdjoinNodes(PathFinding3DProcess process, Node3D from, Func<Node3D, Node3D, bool> moveCheck, List<Node> ret)
        {
            ret.Clear();
            Node3D to;
            foreach (Vector3Int direction in SixDirections)
            {
                to = process.GetNode(from.Position + direction);
                if (moveCheck(from, to))
                    ret.Add(to);
            }
        }

        #endregion

        #region 二十六向寻路（面、边、角相邻）

        public static readonly ReadOnlyCollection<Vector3Int> TwentySixDirections;
        /// <summary>恰好两个分量非零的12个"边"方向（2D对角线，同一坐标平面内斜向相邻）</summary>
        public static readonly ReadOnlyCollection<Vector3Int> EdgeDirections;
        /// <summary>三个分量都非零的8个"角"方向（3D对角线，立方体对角相邻）</summary>
        public static readonly ReadOnlyCollection<Vector3Int> CornerDirections;

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
        /// 获取某节点周围面、边、角相邻的二十六个节点；仿照 <see cref="GetAdjoin8Nodes"/> 的宽松角规则——
        /// 走对角线时，只要绕过障碍物的某一条"侧路"（先走直线分量再拐到目标，或反过来）是畅通的，就允许走：
        /// 边方向（2D对角线）有2条侧路（对应2个直线分量）；角方向（3D对角线）有6条侧路（3个直线分量+3个边分量），任意一条通畅即可
        /// </summary>
        public static void Get26AdjoinNodes(PathFinding3DProcess process, Node3D from, Func<Node3D, Node3D, bool> moveCheck, List<Node> ret)
        {
            ret.Clear();
            Node3D to;

            foreach (Vector3Int direction in SixDirections)
            {
                to = process.GetNode(from.Position + direction);
                if (!moveCheck(from, to))
                    continue;
                ret.Add(to);
            }

            foreach (Vector3Int direction in EdgeDirections)
            {
                to = process.GetNode(from.Position + direction);
                // 边方向恰好2个分量非零，2条侧路分别经由这2个分量各自对应的直线邻格
                Node3D sideX = direction.x != 0 ? process.GetNode(from.Position + new Vector3Int(direction.x, 0, 0)) : null;
                Node3D sideY = direction.y != 0 ? process.GetNode(from.Position + new Vector3Int(0, direction.y, 0)) : null;
                Node3D sideZ = direction.z != 0 ? process.GetNode(from.Position + new Vector3Int(0, 0, direction.z)) : null;
                if (sideX != null && moveCheck(from, sideX) && moveCheck(sideX, to)
                    || sideY != null && moveCheck(from, sideY) && moveCheck(sideY, to)
                    || sideZ != null && moveCheck(from, sideZ) && moveCheck(sideZ, to))
                    ret.Add(to);
            }

            foreach (Vector3Int direction in CornerDirections)
            {
                to = process.GetNode(from.Position + direction);
                // 角方向3个分量都非零，6条侧路分别经由3个直线分量（面）与3个边分量（棱）
                Node3D faceX = process.GetNode(from.Position + new Vector3Int(direction.x, 0, 0));
                Node3D faceY = process.GetNode(from.Position + new Vector3Int(0, direction.y, 0));
                Node3D faceZ = process.GetNode(from.Position + new Vector3Int(0, 0, direction.z));
                Node3D edgeXY = process.GetNode(from.Position + new Vector3Int(direction.x, direction.y, 0));
                Node3D edgeXZ = process.GetNode(from.Position + new Vector3Int(direction.x, 0, direction.z));
                Node3D edgeYZ = process.GetNode(from.Position + new Vector3Int(0, direction.y, direction.z));
                if (moveCheck(from, faceX) && moveCheck(faceX, to)
                    || moveCheck(from, faceY) && moveCheck(faceY, to)
                    || moveCheck(from, faceZ) && moveCheck(faceZ, to)
                    || moveCheck(from, edgeXY) && moveCheck(edgeXY, to)
                    || moveCheck(from, edgeXZ) && moveCheck(edgeXZ, to)
                    || moveCheck(from, edgeYZ) && moveCheck(edgeYZ, to))
                    ret.Add(to);
            }
        }

        #endregion

        /// <summary>
        /// 判断两个方向是否相同（至少有一个0向量时返回false）
        /// </summary>
        public static bool Align(Vector3Int a, Vector3Int b)
        {
            int cross = a.y * b.z - a.z * b.y;
            if (cross != 0)
                return false;
            cross = a.z * b.x - a.x * b.z;
            if (cross != 0) 
                return false;
            cross = a.x * b.y - a.y * b.x;
            if (cross != 0)
                return false;
            int dot = a.x * b.x + a.y * b.y + a.z * b.z;
            return dot > 0;
        }

        /// <summary>
        /// 判断一个方向是否与6个方向之一相同
        /// </summary>
        public static bool Align6(Vector3Int v)
        {
            foreach (Vector3Int direction in SixDirections)
            {
                if(Align(v, direction)) 
                    return true;
            }
            return false;
        }

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

            List<Vector3Int> edge = new();
            List<Vector3Int> corner = new();
            foreach (Vector3Int direction in twentySix)
            {
                int nonZeroCount = (direction.x != 0 ? 1 : 0) + (direction.y != 0 ? 1 : 0) + (direction.z != 0 ? 1 : 0);
                if (nonZeroCount == 2)
                    edge.Add(direction);
                else if (nonZeroCount == 3)
                    corner.Add(direction);
            }
            EdgeDirections = new ReadOnlyCollection<Vector3Int>(edge);
            CornerDirections = new ReadOnlyCollection<Vector3Int>(corner);
        }
    }
}
