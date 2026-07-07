using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static UnityEditor.PlayerSettings;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "跳点", menuName = "AStar2D/获取相邻可达节点的方法/跳点")]
    public class GetJumpPoint2DSO : GetMovableNodes2DSO
    {
        public int depthOnDirection;

        public override void GetMovableNodes(PathFinding2DProcess process, Node2D from, Func<Node2D, Node2D, bool> moveCheck, List<Node> ret)
        {
            void AddToPath(Node2D node)
            {
                process.PersistNode(node);
                //添加到ret的节点，Parent如果为空，则会被设为from;state如果为Blank，则会被设为Open
                ret.Add(node);
            }

            ret.Clear();

            bool FindPathOrthogonal(Node2D start, Vector2Int direction)
            {
                Node2D current = start;
                Vector2Int pos = current.Position;
                bool isJumpPoint = false;
                for (int i = 0; i < depthOnDirection; i++)
                {
                    pos += direction;
                    Node2D next = process.PeekNode(pos);
                    if (!moveCheck(current, next))
                        return false;
                    next.Parent = current; //必须从前往后设置Parent，才能正确计算GCost
                    current = next;

                    isJumpPoint = pos == (process.To as Node2D).Position || GetForcedNeighbourOrthogonal(current, direction);
                    if (isJumpPoint)
                        break;
                }
                isJumpPoint = isJumpPoint || current != start;
                if (isJumpPoint)
                {
                    current.Parent = start;
                    start.state = ENodeState.Close;
                    AddToPath(current);   //走到最大距离的点也视为跳点
                }
                return isJumpPoint;
            }

            bool FindPathDiagonal(Node2D start, Vector2Int direction)
            {
                Node2D current = start;
                Vector2Int pos = current.Position;
                bool isJumpPoint = false;
                for (int i = 0; i < depthOnDirection; i++)
                {
                    pos += direction;
                    Node2D next = process.PeekNode(pos);
                    if (!moveCheck(current, next))
                        return false;
                    next.Parent = current; //必须从前往后设置Parent，才能正确计算GCost
                    current = next;

                    isJumpPoint = pos == (process.To as Node2D).Position
                        || GetForcedNeighbourDiagonal(current, direction)
                        || FindPathOrthogonal(current, new Vector2Int(direction.x, 0))
                        || FindPathOrthogonal(current, new Vector2Int(0, direction.y));
                    if (isJumpPoint)
                        break;
                }
                isJumpPoint = isJumpPoint || current != start;
                if (isJumpPoint)
                {
                    current.Parent = start;
                    start.state = ENodeState.Close;
                    AddToPath(current);   //走到最大距离的点也视为跳点
                }
                return isJumpPoint;
            }

            /// <summary>
            /// 沿非对角线前进时，查找current的强制邻居，如果找到，则直接建立路径
            /// </summary>
            bool GetForcedNeighbourOrthogonal(Node2D current, Vector2Int enterDirection)
            {
                bool hasFocedNeighbour = false;
                Vector2Int[] directions = PathFinding2DUtility.SortedEightDirections[enterDirection];
                Node2D node0 = process.PeekNode(current.Position + directions[0]);
                if (!moveCheck(current, node0))
                    return false;
                Node2D node1 = process.PeekNode(current.Position + directions[1]);
                Node2D node2 = process.PeekNode(current.Position + directions[2]);
                Node2D node3 = process.PeekNode(current.Position + directions[3]);
                Node2D node4 = process.PeekNode(current.Position + directions[4]);

                if (!moveCheck(current, node3) && moveCheck(node0, node1))
                {
                    hasFocedNeighbour = true;
                    node1.Parent = current;
                    AddToPath(node1);
                }
                if (!moveCheck(current, node4) && moveCheck(node0, node2))
                {
                    hasFocedNeighbour = true;
                    node2.Parent = current;
                    AddToPath(node2);
                }

                if (hasFocedNeighbour)
                {
                    node0.Parent = current;
                    current.state = ENodeState.Close;
                    AddToPath(current);
                    AddToPath(node0);
                }

                return hasFocedNeighbour;
            }

            /// <summary>
            /// 沿对角线前进时，查找current的强制邻居，如果找到，则直接建立路径
            /// </summary>
            bool GetForcedNeighbourDiagonal(Node2D current, Vector2Int enterDirection)
            {
                bool hasFocedNeighbour = false;
                Vector2Int[] directions = PathFinding2DUtility.SortedEightDirections[enterDirection];

                Node2D node0 = process.PeekNode(current.Position + directions[0]);
                Node2D node1 = process.PeekNode(current.Position + directions[1]);
                Node2D node2 = process.PeekNode(current.Position + directions[2]);
                Node2D node3 = process.PeekNode(current.Position + directions[3]);
                Node2D node4 = process.PeekNode(current.Position + directions[4]);
                Node2D node5 = process.PeekNode(current.Position + directions[5]);
                Node2D node6 = process.PeekNode(current.Position + directions[6]);
                if (!moveCheck(current, node5) && moveCheck(current, node1) && moveCheck(node1, node3))
                {
                    hasFocedNeighbour = true;
                    node3.Parent = current;
                    node1.Parent = current;
                    AddToPath(node1);
                    AddToPath(node3);
                }
                if (!moveCheck(current, node6) && moveCheck(current, node2) && moveCheck(node2, node4))
                {
                    hasFocedNeighbour = true;
                    node2.Parent = current;
                    node4.Parent = current;
                    AddToPath(node2);
                    AddToPath(node4);
                }

                if (hasFocedNeighbour)
                {
                    current.state = ENodeState.Close;
                    AddToPath(current);
                    if (moveCheck(current, node0))
                    {
                        node0.Parent = current;
                        AddToPath(node0);
                    }
                }

                return hasFocedNeighbour;
            }

            Vector2Int v = from.Parent == null ? Vector2Int.left : from.Position - from.Parent.Position;
            v = new Vector2Int(Math.Sign(v.x), Math.Sign(v.y));
            Vector2Int[] directions = PathFinding2DUtility.SortedEightDirections[v];

            if (from == process.From)
            {
                FindPathOrthogonal(from, directions[0]);
                FindPathOrthogonal(from, directions[1]);
                FindPathDiagonal(from, directions[2]);
                FindPathOrthogonal(from, directions[3]);
                FindPathOrthogonal(from, directions[4]);
                FindPathDiagonal(from, directions[5]);
                FindPathDiagonal(from, directions[6]);
                FindPathOrthogonal(from, directions[7]);
            }
            else if (v.x * v.y != 0)
            {
                FindPathDiagonal(from, directions[0]);
                FindPathOrthogonal(from, directions[1]);
                FindPathOrthogonal(from, directions[2]);
            }
            else
            {
                FindPathOrthogonal(from, directions[0]);
            }
        }
    }
}
