using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "跳点", menuName = "AStar/获取相邻可达节点的方法/跳点")]
    public class GetJumpPointSO : GetMovableNodesSO
    {
        public int depthOnDirection;

        public override void GetMovableNodes(PathFindingProcess process, Node from, Func<Node, Node, bool> moveCheck, List<Node> ret)
        {
            // 沿 from 的"穿不过"判定，统一走 mover.MoveCheck 的语义
            // 这样 Pawn 之类 IsObstacle=false 但 StayCheck 不通过的节点也能被正确处理
            bool CantPass(Vector2Int delta)
            {
                return !moveCheck(from, process.GetNode(from.Position + delta));
            }

            void TryAdd(Node to)
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
                TryAdd(process.GetNode(from.Position + directions[0]));
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[1]));
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[2]));
                if (CantPass(directions[5]))
                    TryAdd(process.GetNode(from.Position + directions[3]));
                if (CantPass(directions[6]))
                    TryAdd(process.GetNode(from.Position + directions[4]));
            }
        }

        public Node FindJumpPointOnDirection(PathFindingProcess process, Vector2Int from, Vector2Int direction)
        {
            Vector2Int current = from;
            Node prev = process.GetNode(from);
            Node node = null;
            for (int i = 0; i < depthOnDirection; i++)
            {
                current += direction;
                node = process.GetNode(current);
                if (!process.mover.MoveCheck(prev, node))
                    return null;
                prev = node;
                if (IsJumpPoint(process, node, direction))
                    return node;
            }
            return node;
        }

        public bool IsJumpPoint(PathFindingProcess process, Node node, Vector2Int direction)
        {
            bool CantPass(Vector2Int delta)
            {
                return !process.mover.MoveCheck(node, process.GetNode(node.Position + delta));
            }

            if (node == process.To)
                return true;

            PathFinding2DUtility.Comparer_Vector2_Nearer comparer = new(direction);
            Vector2Int[] directions = PathFinding2DUtility.EightDirections.ToArray();
            Array.Sort(directions, comparer);

            return CantPass(directions[3]) && !CantPass(directions[1]) ||
                CantPass(directions[4]) && !CantPass(directions[2]);
        }
    }
}