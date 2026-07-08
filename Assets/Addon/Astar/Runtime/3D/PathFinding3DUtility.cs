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

        public static readonly ReadOnlyCollection<Vector3Int> Orthogonals;

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
            foreach (Vector3Int direction in Orthogonals)
            {
                to = process.GetNode(from.Position + direction);
                if (moveCheck(from, to))
                    ret.Add(to);
            }
        }

        #endregion

        #region 二十六向寻路（面、边、角相邻）

        public static readonly ReadOnlyCollection<Vector3Int> TwentySixDirections;
        //与面对角线平行的方向
        public static readonly ReadOnlyCollection<Vector3Int> FaceDiagonalDirections;
        //体对角线方向
        public static readonly ReadOnlyCollection<Vector3Int> BodyDiagonalDirections;

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

            foreach (Vector3Int direction in Orthogonals)
            {
                to = process.GetNode(from.Position + direction);
                if (!moveCheck(from, to))
                    continue;
                ret.Add(to);
            }

            foreach (Vector3Int direction in FaceDiagonalDirections)
            {
                to = process.GetNode(from.Position + direction);
                // 边方向恰好2个分量非零，2条侧路分别经由这2个分量各自对应的直线邻格
                Node3D sideX = direction.x != 0 ? process.GetNode(from.Position + new Vector3Int(direction.x, 0, 0)) : null;
                Node3D sideY = direction.y != 0 ? process.GetNode(from.Position + new Vector3Int(0, direction.y, 0)) : null;
                Node3D sideZ = direction.z != 0 ? process.GetNode(from.Position + new Vector3Int(0, 0, direction.z)) : null;
                if (moveCheck(from, sideX) && moveCheck(sideX, to)
                    || moveCheck(from, sideY) && moveCheck(sideY, to)
                    || moveCheck(from, sideZ) && moveCheck(sideZ, to))
                    ret.Add(to);
            }

            foreach (Vector3Int direction in BodyDiagonalDirections)
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

        public static readonly Dictionary<Vector3Int, Vector3Int[]> SortedTwentySixDirections;

        /// <summary>
        /// 对向量按与某个向量的夹角大小排序（3D版，用于JPS按"最接近来向"的顺序遍历26个方向）；
        /// 2D版（<see cref="AStar.TwoD.PathFinding2DUtility.Comparer_Vector2_Nearer"/>）夹角相同时只需要
        /// 一个正负号区分"绕direction的哪一侧"，因为2D里垂直于direction的截面退化成两个点；
        /// 3D里垂直于direction的截面是一整圈，必须用完整的"绕direction的钟表角"才能确定唯一顺序，
        /// 否则夹角相同的一批方向（比如45°的4个边方向）彼此之间的先后顺序会不确定
        /// </summary>
        public class Comparer_Vector3_Nearer : IComparer<Vector3>, IComparer<Vector3Int>
        {
            public readonly Vector3 direction;
            private readonly Vector3 axis1;
            private readonly Vector3 axis2;

            public Comparer_Vector3_Nearer(Vector3 direction)
            {
                this.direction = direction.normalized;
                // 在垂直于direction的平面内取一对正交基(axis1,axis2)：先用世界上方向对direction做Gram-Schmidt投影得到axis1，
                // 如果direction本身接近竖直（几乎与上方向平行/反平行），这个投影会退化为零向量，改用世界前方向兜底
                Vector3 seed = Mathf.Abs(Vector3.Dot(this.direction, Vector3.up)) > 0.999f ? Vector3.forward : Vector3.up;
                axis1 = (seed - this.direction * Vector3.Dot(seed, this.direction)).normalized;
                axis2 = Vector3.Cross(this.direction, axis1);
            }

            /// <summary>候选方向投影到(axis1,axis2)平面后，绕direction的钟表角，范围[0, 2π)</summary>
            private float ClockAngle(Vector3 x)
            {
                float angle = Mathf.Atan2(Vector3.Dot(x, axis2), Vector3.Dot(x, axis1));
                return angle < 0 ? angle + Mathf.PI * 2 : angle;
            }

            public int Compare(Vector3 x, Vector3 y)
            {
                float angleX = Vector3.Angle(direction, x);
                float angleY = Vector3.Angle(direction, y);
                if (angleX != angleY)
                    return angleX.CompareTo(angleY);
                return ClockAngle(x).CompareTo(ClockAngle(y));
            }

            public int Compare(Vector3Int x, Vector3Int y)
            {
                return Compare((Vector3)x, (Vector3)y);
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
            Orthogonals = new ReadOnlyCollection<Vector3Int>(six);

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
            FaceDiagonalDirections = new ReadOnlyCollection<Vector3Int>(edge);
            BodyDiagonalDirections = new ReadOnlyCollection<Vector3Int>(corner);

            SortedTwentySixDirections = new();
            foreach (Vector3Int direction in twentySix)
            {
                Vector3Int[] directions = twentySix.ToArray();
                Comparer_Vector3_Nearer comparer = new(direction);
                Array.Sort(directions, comparer);
                SortedTwentySixDirections.Add(direction, directions);
            }
        }
    }
}
