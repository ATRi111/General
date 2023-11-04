using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "跳点")]
    public class GetJumpPointSO : GetAdjoinedNodesSO
    {
        public int depthOnDirection;

        public override void GetAdjoinedNodes(PathFindingProcess process, PathNode node, List<PathNode> ret)
        {
            bool MatchType(Vector2Int direction, ENodeType type)
            {
                return process.GetNode(node.Position + direction).Type == type;
            }

            void TryAdd(PathNode node)
            {
                if (node != null)
                    ret.Add(node);
            }

            Vector2Int[] directions = PathFindingUtility.eightDirections.ToArray();
            Vector2Int v = node.Parent == null ? Vector2Int.left : node.Position - node.Parent.Position;
            PathFindingUtility.Comparer_Vector2_Nearer comparer = new PathFindingUtility.Comparer_Vector2_Nearer(v);
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
                if (MatchType(directions[3], ENodeType.Obstacle))
                    TryAdd(process.GetNode(node.Position + directions[1]));
                if (MatchType(directions[4], ENodeType.Obstacle))
                    TryAdd(process.GetNode(node.Position + directions[2]));
            }
            else
            {
                TryAdd(process.GetNode(node.Position + directions[0]));
                TryAdd(FindJumpPointOnDirection(process, node.Position, directions[1]));
                TryAdd(FindJumpPointOnDirection(process, node.Position, directions[2]));
                if (MatchType(directions[5], ENodeType.Obstacle))
                    TryAdd(process.GetNode(node.Position + directions[3]));
                if (MatchType(directions[6], ENodeType.Obstacle))
                    TryAdd(process.GetNode(node.Position + directions[4]));
            }
        }

        public PathNode FindJumpPointOnDirection(PathFindingProcess process, Vector2Int from, Vector2Int direction)
        {
            Vector2Int current = from;
            PathNode node;
            for (int i = 0; i < depthOnDirection; i++)
            {
                current += direction;
                node = process.GetNode(current);
                if (node.Type == ENodeType.Obstacle)
                    return null;
                if (IsJumpPoint(process, node, direction))
                    return node;
            }
            node = process.GetNode(current);
            if (node.Type == ENodeType.Blank)
                return node;
            return null;
        }

        public bool IsJumpPoint(PathFindingProcess process, PathNode node, Vector2Int direction)
        {
            bool MatchType(Vector2Int direction, ENodeType type)
            {
                return process.GetNode(node.Position + direction).Type == type;
            }

            switch (node.Type)
            {
                case ENodeType.Route:
                    return true;
                case ENodeType.Obstacle:
                case ENodeType.Close:
                    return false;
            }

            PathFindingUtility.Comparer_Vector2_Nearer comparer = new PathFindingUtility.Comparer_Vector2_Nearer(direction);
            Vector2Int[] directions = PathFindingUtility.eightDirections.ToArray();
            Array.Sort(directions, comparer);

            return MatchType(directions[3], ENodeType.Obstacle) && MatchType(directions[1], ENodeType.Blank) ||
                MatchType(directions[4], ENodeType.Obstacle) && MatchType(directions[2], ENodeType.Blank);
        }
    }
}