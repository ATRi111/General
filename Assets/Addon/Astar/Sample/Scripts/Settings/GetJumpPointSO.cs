using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar.Sample
{
    [CreateAssetMenu(fileName = "跳点")]
    public class GetJumpPointSO : GetAdjoinedNodesSO
    {
        public int depthOnDirection;

        public override void GetAdjoinedNodes(PathFindingProcess process, AStarNode node, List<AStarNode> ret)
        {
            bool IsObstacle(Vector2Int delta)
            {
                return process.GetNode(node.Position + delta).IsObstacle;
            }

            void TryAdd(AStarNode node)
            {
                if (node != null)
                    ret.Add(node);
            }

            Vector2Int[] directions = PathFindingUtility.eightDirections.ToArray();
            Vector2Int v = node.Parent == null ? Vector2Int.left : node.Position - node.Parent.Position;
            PathFindingUtility.Comparer_Vector2_Nearer comparer = new(v);
            Array.Sort(directions, comparer);

            if (node.Parent == null)
            {
                TryAdd(FindJumpPointOnDirection(process, node.Position, directions[0]));
                TryAdd(FindJumpPointOnDirection(process, node.Position, directions[3]));
                TryAdd(FindJumpPointOnDirection(process, node.Position, directions[4]));
                TryAdd(FindJumpPointOnDirection(process, node.Position, directions[7]));
                TryAdd(process.GetNode(node.Position + directions[1]));
                TryAdd(process.GetNode(node.Position + directions[2]));
                TryAdd(process.GetNode(node.Position + directions[5]));
                TryAdd(process.GetNode(node.Position + directions[6]));
                return;
            }

            if (v.x == 0 || v.y == 0)
            {
                TryAdd(FindJumpPointOnDirection(process, node.Position, directions[0]));
                if (IsObstacle(directions[3]))
                    TryAdd(process.GetNode(node.Position + directions[1]));
                if (IsObstacle(directions[4]))
                    TryAdd(process.GetNode(node.Position + directions[2]));
            }
            else
            {
                TryAdd(process.GetNode(node.Position + directions[0]));
                TryAdd(FindJumpPointOnDirection(process, node.Position, directions[1]));
                TryAdd(FindJumpPointOnDirection(process, node.Position, directions[2]));
                if (IsObstacle(directions[5]))
                    TryAdd(process.GetNode(node.Position + directions[3]));
                if (IsObstacle(directions[6]))
                    TryAdd(process.GetNode(node.Position + directions[4]));
            }
        }

        public AStarNode FindJumpPointOnDirection(PathFindingProcess process, Vector2Int from, Vector2Int direction)
        {
            Vector2Int current = from;
            AStarNode node;
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

        public bool IsJumpPoint(PathFindingProcess process, AStarNode node, Vector2Int direction)
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
            Vector2Int[] directions = PathFindingUtility.eightDirections.ToArray();
            Array.Sort(directions, comparer);

            return IsObstacle(directions[3]) && IsBlank(directions[1]) ||
                IsObstacle(directions[4]) && IsBlank(directions[2]);
        }
    }
}