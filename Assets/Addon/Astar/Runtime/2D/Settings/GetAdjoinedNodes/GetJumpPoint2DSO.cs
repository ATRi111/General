using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "跳点", menuName = "AStar2D/获取相邻可达节点的方法/跳点")]
    public class GetJumpPoint2DSO : GetMovableNodes2DSO
    {
        public int depthOnDirection;
        public bool lazyScan;

        public override void GetMovableNodes(PathFinding2DProcess process, Node2D from, Func<Node2D, Node2D, bool> moveCheck, List<Node> ret)
        {
            void AddToPath(Node2D node)
            {
                //此函数不能用于添加终点
                if (!process.cachedNodes.ContainsKey(node.Position))
                {
                    process.PersistNode(node);
                    //添加到ret的节点，Parent如果为空，则会被设为from;state如果为Blank，则会被设为Open
                    ret.Add(node);
                }
            }

            bool FindPathOrthogonal(Node2D start, Vector2Int direction)
            {
                Node2D current = start;
                Vector2Int pos = current.Position;
                bool isJumpPoint = false;
                for (int i = 0; i < depthOnDirection; i++)
                {
                    pos += direction;
                    Node2D next = process.PeekNode(pos);

                    if (!process.ExploreCheck(current, next))
                        return false;

                    current = next;
                    current.UpdateParent(start);    //必须正确地从前往后设置Parent

                    if (current == process.To)
                    {
                        ret.Add(current);
                        return true;
                    }

                    isJumpPoint = GetForcedNeighbourOrthogonal(current, direction);
                    if (isJumpPoint)
                        break;
                }
                isJumpPoint |= current != start;    //走到最大距离的点也作为预知对角线上跳点的依据（否则，沿对角线前进时，会一次性进行大量预测）
                if (isJumpPoint)
                    AddToPath(current);
                return isJumpPoint;
            }


            bool FindPathDiagonal(Node2D start, Vector2Int direction)
            {
                return lazyScan ? FindPathDiagonal_Lazy(start, direction) : FindPathDiagonal_Regular(start, direction);
            }

            bool FindPathDiagonal_Regular(Node2D start, Vector2Int direction)
            {
                Node2D current = start;
                bool found = false;
                Node2D horizontal, vertical, next;
                int bitMask;
                for (int i = 0; i < depthOnDirection; i++)
                {
                    bitMask = PathFinding2DUtility.DiagonalMoveCheck(process, current, process.ExploreCheck, direction,
                            out next, out horizontal, out vertical);

                    if ((bitMask & 0b100) == 0)
                        return false;

                    current = next;
                    current.UpdateParent(start);    //必须正确地从前往后设置Parent

                    if (current == process.To)
                    {
                        ret.Add(current);
                        return true;
                    }

                    found = GetForcedNeighbourDiagonal(current, direction)
                        || (bitMask & 0b1) > 0 && FindPathOrthogonal(current, new Vector2Int(direction.x, 0))
                        || (bitMask & 0b10) > 0 && FindPathOrthogonal(current, new Vector2Int(0, direction.y));
                    if (found)
                        break;
                }
                found |= current != start;
                if (found)
                    AddToPath(current);
                return found;
            }

            bool FindPathDiagonal_Lazy(Node2D start, Vector2Int direction)
            {
                Node2D current = start;
                Node2D horizontal, vertical, next;

                int bitMask = PathFinding2DUtility.DiagonalMoveCheck(process, current, process.ExploreCheck, direction,
                            out next, out horizontal, out vertical);
                if ((bitMask & 0b100) == 0)
                    return false;
                current = next;
                current.UpdateParent(start);

                for (int i = 0; i < depthOnDirection; i++)
                {
                    AddToPath(current);
                    if (current == process.To)
                    {
                        ret.Add(current);
                        return true;
                    }

                    bitMask = PathFinding2DUtility.DiagonalMoveCheck(process, current, process.ExploreCheck, direction,
                            out next, out horizontal, out vertical);

                    if ((bitMask & 0b1) > 0)
                    {
                        horizontal.UpdateParent(current);
                        AddToPath(horizontal);
                    }
                    if ((bitMask & 0b10) > 0)
                    {
                        vertical.UpdateParent(current);
                        AddToPath(vertical);
                    }
                    if ((bitMask & 0b100) > 0)  
                    {
                        next.UpdateParent(current);
                        AddToPath(next);
                    }
                    current.state = ENodeState.Close;
                    if (GetForcedNeighbourDiagonal(current, direction))
                        break;
                    if ((bitMask & 0b100) == 0)
                        break;
                    current = next;
                }
                return true;
            }

            //获取单一方向上的强制邻居
            bool GetForcedNeighbour(Node2D current, Vector2Int direction, Vector2Int expectedPass)
            {
                int bitMask = PathFinding2DUtility.DiagonalMoveCheck(process, current, moveCheck, direction,
                            out Node2D next, out Node2D horizontal, out Node2D vertical);
                int expectedValue = expectedPass.x != 0 ? 0b101 : 0b110;
                if (bitMask == expectedValue)
                {
                    next.UpdateParent(current);
                    AddToPath(next);
                    return true;
                }
                return false;
            }

            bool GetForcedNeighbourOrthogonal(Node2D current, Vector2Int enterDirection)
            {
                bool hasFocedNeighbour = false;
                Vector2Int[] directions = PathFinding2DUtility.SortedEightDirections[enterDirection];

                hasFocedNeighbour |= GetForcedNeighbour(current, directions[1], directions[0]);
                hasFocedNeighbour |= GetForcedNeighbour(current, directions[2], directions[0]);

                if (hasFocedNeighbour)
                    AddToPath(current);

                return hasFocedNeighbour;
            }

            bool GetForcedNeighbourDiagonal(Node2D current, Vector2Int enterDirection)
            {
                bool hasFocedNeighbour = false;
                Vector2Int[] directions = PathFinding2DUtility.SortedEightDirections[enterDirection];

                hasFocedNeighbour |= GetForcedNeighbour(current, directions[3], directions[1]);
                hasFocedNeighbour |= GetForcedNeighbour(current, directions[4], directions[2]);

                if (hasFocedNeighbour)
                    AddToPath(current);

                return hasFocedNeighbour;
            }

            if (from == process.From)
            {
                foreach(Vector2Int direction in PathFinding2DUtility.Orthogonals)
                {
                    FindPathOrthogonal(from, direction);
                }
                foreach(Vector2Int direction in PathFinding2DUtility.Diagonals)
                {
                    FindPathDiagonal(from, direction);
                }
                return;
            }


            ret.Clear();

            Vector2Int v = from.Position - from.Parent.Position;
            v = new Vector2Int(Math.Sign(v.x), Math.Sign(v.y));
            Vector2Int[] directions = PathFinding2DUtility.SortedEightDirections[v];

            if (v.x * v.y != 0)
            {
                GetForcedNeighbourDiagonal(from, v);
                FindPathDiagonal(from, directions[0]);
                FindPathOrthogonal(from, directions[1]);
                FindPathOrthogonal(from, directions[2]);
            }
            else
            {
                GetForcedNeighbourOrthogonal(from, v);
                FindPathOrthogonal(from, directions[0]);
            }
        }
    }
}
