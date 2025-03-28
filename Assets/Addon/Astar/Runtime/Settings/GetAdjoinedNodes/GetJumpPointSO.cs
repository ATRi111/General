using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar.Sample
{
    [CreateAssetMenu(fileName = "跳点", menuName = "AStar/获取相邻可达节点的方法/跳点")]
    public class GetJumpPointSO : GetMovableNodesSO
    {
        public int depthOnDirection;

        public override void GetMovableNodes(PathFindingProcess process, Node from, Func<Node, Node, bool> moveCheck, List<Node> ret)
        {
            bool IsObstacle(Vector2Int delta)
            {
                return process.GetNode(from.Position + delta).IsObstacle;
            }

            void TryAdd(Node to)
            {
                if (to != null && moveCheck(from,to))
                    ret.Add(to);
            }

            Vector2Int[] directions = PathFindingUtility.EightDirections.ToArray();
            Vector2Int v = from.Parent == null ? Vector2Int.left : from.Position - from.Parent.Position;
            PathFindingUtility.Comparer_Vector2_Nearer comparer = new(v);
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
                if (IsObstacle(directions[3]))
                    TryAdd(process.GetNode(from.Position + directions[1]));
                if (IsObstacle(directions[4]))
                    TryAdd(process.GetNode(from.Position + directions[2]));
            }
            else
            {
                TryAdd(process.GetNode(from.Position + directions[0]));
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[1]));
                TryAdd(FindJumpPointOnDirection(process, from.Position, directions[2]));
                if (IsObstacle(directions[5]))
                    TryAdd(process.GetNode(from.Position + directions[3]));
                if (IsObstacle(directions[6]))
                    TryAdd(process.GetNode(from.Position + directions[4]));
            }
        }

        public Node FindJumpPointOnDirection(PathFindingProcess process, Vector2Int from, Vector2Int direction)
        {
            Vector2Int current = from;
            Node node;
            for (int i = 0; i < depthOnDirection; i++)
            {
                current += direction;
                node = process.GetNode(current);
                if (node.IsObstacle)
                    return null;
                if (IsJumpPoint(process, node, direction))
                    return node;
            }
            node = process.GetNode(current);
            if (node.state == ENodeState.Blank)
                return node;
            return null;
        }

        public bool IsJumpPoint(PathFindingProcess process, Node node, Vector2Int direction)
        {
            bool IsBlank(Vector2Int delta)
            {
                return process.GetNode(node.Position + delta).state == ENodeState.Blank;
            }
            bool IsObstacle(Vector2Int delta)
            {
                return process.GetNode(node.Position + delta).IsObstacle;
            }

            if (node == process.To)
                return true;
            if (node.IsObstacle || node.state == ENodeState.Close)
                return false;

            PathFindingUtility.Comparer_Vector2_Nearer comparer = new(direction);
            Vector2Int[] directions = PathFindingUtility.EightDirections.ToArray();
            Array.Sort(directions, comparer);

            return IsObstacle(directions[3]) && IsBlank(directions[1]) ||
                IsObstacle(directions[4]) && IsBlank(directions[2]);
        }
    }
}