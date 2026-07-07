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

            bool CantPass(Node2D from, Vector2Int delta)
            {
                Node2D peek = process.PeekNode(from.Position + delta);
                return peek == null || !moveCheck(from, peek);
            }

            /// <summary>
            /// 沿某个方向扫描寻找跳点
            /// </summary>
            Node FindJumpPointOnDirection(Node2D from, Vector2Int direction)
            {
                Node2D current = from;
                Vector2Int pos = current.Position;
                for (int i = 0; i < depthOnDirection; i++)
                {
                    pos += direction;
                    Node2D next = process.PeekNode(pos);
                    if (next == null || !process.mover.MoveCheck(current, next))
                        return null;
                    current = next;
                    if (IsJumpPoint(current, direction))
                    {
                        switch (from.state)
                        {
                            //父节点是Open中刚取出的节点的情况
                            case ENodeState.Open:
                                process.PersistNode(current);
                                //如果current是沿对角线前进找到的跳点，到这里current的state可能已经是Close，这种情况下process不会重新把节点加到Open里
                                ret.Add(current);
                                break;
                            //父节点是沿对角线前进时找到的待定节点的情况
                            case ENodeState.Blank:
                                process.PersistNode(current);
                                current.Parent = from;
                                from.state = ENodeState.Close;
                                //current已经有Parent，所以process不会错误地把Parent设为一开始取出的Open节点
                                ret.Add(current);
                                break;
                            case ENodeState.Close:
                                throw new InvalidOperationException();
                        }
                        return current;
                    }
                }
                if (current != from)
                {
                    process.PersistNode(current);
                    ret.Add(current);   //走到最大距离的点也视为跳点
                    return current;
                }
                return null;
            }

            bool IsJumpPoint(Node2D node, Vector2Int direction)
            {
                // 终点也被视为跳点
                if (node.Position == (process.To as Node2D).Position)
                    return true;

                Vector2Int[] directions = PathFinding2DUtility.SortedEightDirections[direction];

                //因障碍物而产生的跳点
                if (CantPass(node, directions[3]) && !CantPass(node, directions[1]) ||
                    CantPass(node, directions[4]) && !CantPass(node, directions[2]))
                    return true;

                // 沿对角线方向移动时，需额外检查水平竖直方向，如果至少一个方向上有跳点，则当前点也是跳点
                if (direction.x != 0 && direction.y != 0)
                {
                    Vector2Int horizontal = new(direction.x, 0);
                    Vector2Int vertical = new(0, direction.y);
                    Node next = FindJumpPointOnDirection(node, horizontal);
                    next ??= FindJumpPointOnDirection(node, vertical);
                    return next != null;
                }

                return false;
            }

            void AddMovable(Node2D to)
            {
                if (to != null && moveCheck(from, to))
                    ret.Add(to);
            }

            Vector2Int v = from.Parent == null ? Vector2Int.left : from.Position - from.Parent.Position;
            v = new Vector2Int(Math.Sign(v.x), Math.Sign(v.y));
            Vector2Int[] directions = PathFinding2DUtility.SortedEightDirections[v];

            if (from.Parent == null)
            {
                FindJumpPointOnDirection(from, directions[0]);
                FindJumpPointOnDirection(from, directions[1]);
                FindJumpPointOnDirection(from, directions[2]);
                FindJumpPointOnDirection(from, directions[3]);
                FindJumpPointOnDirection(from, directions[4]);
                FindJumpPointOnDirection(from, directions[5]);
                FindJumpPointOnDirection(from, directions[6]);
                FindJumpPointOnDirection(from, directions[7]);
            }
            else if (v.x == 0 || v.y == 0)
            {
                FindJumpPointOnDirection(from, directions[0]);
                if (CantPass(from, directions[3]))
                    AddMovable(process.GetNode(from.Position + directions[1]));
                if (CantPass(from, directions[4]))
                    AddMovable(process.GetNode(from.Position + directions[2]));
            }
            else
            {
                FindJumpPointOnDirection(from, directions[0]);
                FindJumpPointOnDirection(from, directions[1]);
                FindJumpPointOnDirection(from, directions[2]);
                if (CantPass(from, directions[5]))
                    AddMovable(process.GetNode(from.Position + directions[3]));
                if (CantPass(from, directions[6]))
                    AddMovable(process.GetNode(from.Position + directions[4]));
            }
        }
    }
}
