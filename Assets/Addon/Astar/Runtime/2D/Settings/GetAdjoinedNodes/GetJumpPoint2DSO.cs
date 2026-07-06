using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "跳点", menuName = "AStar2D/获取相邻可达节点的方法/跳点")]
    public class GetJumpPoint2DSO : GetMovableNodes2DSO
    {
        public int depthOnDirection;

        public override void GetMovableNodes(PathFinding2DProcess process, Node2D from, Func<Node2D, Node2D, bool> moveCheck, List<Node> ret)
        {
            ret.Clear();

            bool CantPass(Vector2Int delta)
            {
                Node2D peek = process.PeekNode(from.Position + delta);
                return peek == null || !moveCheck(from, peek);
            }

            void TryAdd(Node2D to)
            {
                if (to != null && moveCheck(from, to))
                    ret.Add(to);
            }

            Vector2Int[] directions = PathFinding2DUtility.EightDirections.ToArray();
            Vector2Int v = from.Parent == null ? Vector2Int.left : from.Position - from.Parent.Position;
            PathFinding2DUtility.Comparer_Vector2_Nearer comparer = new(v);
            Array.Sort(directions, comparer);

            if (from.Parent == null)
            {
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[0]));
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[3]));
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[4]));
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[7]));
                TryAdd(process.GetNode(from.Position + directions[1]));
                TryAdd(process.GetNode(from.Position + directions[2]));
                TryAdd(process.GetNode(from.Position + directions[5]));
                TryAdd(process.GetNode(from.Position + directions[6]));
                return;
            }

            if (v.x == 0 || v.y == 0)
            {
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[0]));
                if (CantPass(directions[3]))
                    TryAdd(process.GetNode(from.Position + directions[1]));
                if (CantPass(directions[4]))
                    TryAdd(process.GetNode(from.Position + directions[2]));
            }
            else
            {
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[0]));
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[1]));
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[2]));
                if (CantPass(directions[5]))
                    TryAdd(process.GetNode(from.Position + directions[3]));
                if (CantPass(directions[6]))
                    TryAdd(process.GetNode(from.Position + directions[4]));
            }
        }

        /// <summary>
        /// 沿某个方向扫描寻找跳点位置，仅用于探测（不缓存扫描到的中间节点），
        /// 返回 null 表示未找到（撞到障碍物，或前进距离为 0）
        /// </summary>
        private Vector2Int? PeekJumpPointOnDirection(PathFinding2DProcess process, Vector2Int from, Vector2Int direction)
        {
            Vector2Int current = from;
            Node2D prev = process.PeekNode(from);
            bool moved = false;
            for (int i = 0; i < depthOnDirection; i++)
            {
                current += direction;
                Node2D probe = process.PeekNode(current);
                if (probe == null || !process.mover.MoveCheck(prev, probe))
                    return null;
                prev = probe;
                moved = true;
                if (IsJumpPoint(process, probe, direction))
                    return current;
            }
            return moved ? current : null;
        }

        /// <summary>
        /// 沿某个方向扫描寻找跳点。扫描途中经过的中间格子只用于障碍判定（不缓存），
        /// 只有最终确定要返回的位置（跳点本身，或达到搜索深度上限时的兜底位置）才会转换为真正缓存的节点
        /// </summary>
        public Node2D FindJumpPointOnDirection(PathFinding2DProcess process, Vector2Int from, Vector2Int direction)
        {
            Vector2Int? found = PeekJumpPointOnDirection(process, from, direction);
            return found.HasValue ? process.GetNode(found.Value) : null;
        }

        public bool IsJumpPoint(PathFinding2DProcess process, Node2D node, Vector2Int direction)
        {
            bool CantPass(Vector2Int delta)
            {
                Node2D peek = process.PeekNode(node.Position + delta);
                return peek == null || !process.mover.MoveCheck(node, peek);
            }

            // node 可能是 PeekNode 探测出的临时节点，与 process.To 比较需按位置而非引用判断
            if (node.Position == ((Node2D)process.To).Position)
                return true;

            PathFinding2DUtility.Comparer_Vector2_Nearer comparer = new(direction);
            Vector2Int[] directions = PathFinding2DUtility.EightDirections.ToArray();
            Array.Sort(directions, comparer);

            if (CantPass(directions[3]) && !CantPass(directions[1]) ||
                CantPass(directions[4]) && !CantPass(directions[2]))
                return true;

            // 沿对角线方向移动时，若从当前节点出发沿任一正交分量能找到跳点，
            // 当前节点本身即为转折点（需继续在此处拆分为多个方向搜索），而不能再往对角线深处跳过去
            if (direction.x != 0 && direction.y != 0)
            {
                Vector2Int horizontal = new(direction.x, 0);
                Vector2Int vertical = new(0, direction.y);
                if (PeekJumpPointOnDirection(process, node.Position, horizontal).HasValue)
                    return true;
                if (PeekJumpPointOnDirection(process, node.Position, vertical).HasValue)
                    return true;
            }

            return false;
        }
    }
}
