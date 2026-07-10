using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.ThreeD
{
    [CreateAssetMenu(fileName = "跳点", menuName = "AStar3D/获取相邻可达节点的方法/六向跳点")]
    public class GetJumpPoint3DSO : GetMovableNodes3DSO
    {
        public ScanCheck3DSO ScanSO;

        public bool lazyScan;

        public override void GetMovableNodes(PathFinding3DProcess process, Node3D from, Func<Node3D, Node3D, bool> moveCheck, List<Node> ret)
        {
            void AddToPath(Node3D node)
            {
                //此函数不能用于添加终点
                if (!process.cachedNodes.ContainsKey(node.Position))
                {
                    process.PersistNode(node);
                    //添加到ret的节点，Parent如果为空，则会被设为from;state如果为Blank，则会被设为Open
                    ret.Add(node);
                }
            }

            // direction恰好有两个非零分量(面对角线)时，拆成两个直线自然分量a、b（哪个分量恰好是0，就说明
            // 另外两个分量组成了这个面对角线所在的坐标面）；FindPathFaceDiagonal、GetForcedNeighbourFaceDiagonal、
            // 以及dispatch的case 2都需要同一份拆分，统一到这里，避免同样的if/else写三遍
            void SplitFaceDiagonal(Vector3Int direction, out Vector3Int a, out Vector3Int b)
            {
                if (direction.z == 0) { a = new Vector3Int(direction.x, 0, 0); b = new Vector3Int(0, direction.y, 0); }
                else if (direction.y == 0) { a = new Vector3Int(direction.x, 0, 0); b = new Vector3Int(0, 0, direction.z); }
                else { a = new Vector3Int(0, direction.y, 0); b = new Vector3Int(0, 0, direction.z); }
            }

            bool FindPathOrthogonal(Node3D start, Vector3Int direction)
            {
                Node3D current = start;
                bool found = false;
                for (Vector3Int pos = current.Position + direction; ; pos += direction)
                {
                    Node3D next = process.PeekNode(pos);
                    if (!process.ExploreCheck(current, next))
                        return false;

                    current = next;
                    current.UpdateParent(start);    //必须正确地从前往后设置Parent

                    if (current == process.To)
                    {
                        ret.Add(current);
                        return true;
                    }

                    found = GetForcedNeighbourOrthogonal(current, direction)
                        || !ScanSO.ScanCheck(process, start, current);
                    if (found)
                    {
                        AddToPath(current);
                        break;
                    }
                }
                return found;
            }

            bool FindPathFaceDiagonal(Node3D start, Vector3Int direction)
                => lazyScan ? FindPathFaceDiagonal_Lazy(start, direction) : FindPathFaceDiagonal_Regular(start, direction);

            bool FindPathFaceDiagonal_Regular(Node3D start, Vector3Int direction)
            {
                // 只依赖direction本身，扫描全程不变，循环外拆一次即可
                SplitFaceDiagonal(direction, out Vector3Int a, out Vector3Int b);

                Node3D current = start;
                bool found = false;
                Node3D nodeA, nodeB, next;
                int bitMask;
                for (; ; )
                {
                    bitMask = PathFinding3DUtility.FaceDiagonalMoveCheck(process, current, process.ExploreCheck, a, b, direction,
                        out next, out nodeA, out nodeB);

                    if ((bitMask & 0b100) == 0)
                        return false;

                    current = next;
                    current.UpdateParent(start);    //必须正确地从前往后设置Parent

                    if (current == process.To)
                    {
                        ret.Add(current);
                        return true;
                    }

                    found = GetForcedNeighbourFaceDiagonal(current, direction)
                        || (bitMask & 0b1) > 0 && FindPathOrthogonal(current, a)
                        || (bitMask & 0b10) > 0 && FindPathOrthogonal(current, b)
                        || !ScanSO.ScanCheck(process, start, current);
                    if (found)
                    {
                        AddToPath(current);
                        break;
                    }
                }
                return found;
            }

            bool FindPathFaceDiagonal_Lazy(Node3D start, Vector3Int direction)
            {
                SplitFaceDiagonal(direction, out Vector3Int a, out Vector3Int b);

                Node3D current = start;
                Node3D nodeA, nodeB, next;

                int bitMask = PathFinding3DUtility.FaceDiagonalMoveCheck(process, current, process.ExploreCheck, a, b, direction,
                    out next, out nodeA, out nodeB);
                if ((bitMask & 0b100) == 0)
                    return false;
                current = next;
                current.UpdateParent(start);

                for (; ; )
                {
                    AddToPath(current);
                    if (current == process.To)
                    {
                        ret.Add(current);
                        return true;
                    }

                    bitMask = PathFinding3DUtility.FaceDiagonalMoveCheck(process, current, process.ExploreCheck, a, b, direction,
                        out next, out nodeA, out nodeB);

                    // 侧向两个分量各挂1步relay，不做完整的递归子扫描——relay自己被Open堆弹出时，
                    // 会按其Parent方向（纯正交）自动继续做完整扫描
                    if ((bitMask & 0b1) > 0)
                    {
                        nodeA.UpdateParent(current);
                        AddToPath(nodeA);
                    }
                    if ((bitMask & 0b10) > 0)
                    {
                        nodeB.UpdateParent(current);
                        AddToPath(nodeB);
                    }
                    if ((bitMask & 0b100) > 0)
                    {
                        next.UpdateParent(current);
                        AddToPath(next);
                    }
                    current.state = ENodeState.Close;
                    if (GetForcedNeighbourFaceDiagonal(current, direction))
                        break;
                    if ((bitMask & 0b100) == 0)
                        break;
                    current = next;
                    if (!ScanSO.ScanCheck(process, start, current))
                        break;
                }
                return true;
            }

            bool FindPathBodyDiagonal(Node3D start, Vector3Int direction)
                => lazyScan ? FindPathBodyDiagonal_Lazy(start, direction) : FindPathBodyDiagonal_Regular(start, direction);

            bool FindPathBodyDiagonal_Regular(Node3D start, Vector3Int direction)
            {
                Node3D current = start;
                bool found = false;
                Node3D ax, ay, az, axay, axaz, ayaz, next;
                int bitMask;
                for (; ; )
                {
                    bitMask = PathFinding3DUtility.BodyDiagonalMoveCheck(process, current, moveCheck, direction,
                        out next, out ax, out ay, out az, out axay, out axaz, out ayaz);

                    if ((bitMask & 0b1000000) == 0)   //合理的路径不可能互相穿插
                        return false;

                    current = next;
                    current.UpdateParent(start);    //必须正确地从前往后设置Parent

                    if (current == process.To)
                    {
                        ret.Add(current);
                        return true;
                    }

                    found = GetForcedNeighbourBodyDiagonal(current, direction)
                        || (bitMask & 0b1000) > 0 && FindPathFaceDiagonal(current, new Vector3Int(direction.x, direction.y, 0))
                        || (bitMask & 0b10000) > 0 && FindPathFaceDiagonal(current, new Vector3Int(direction.x, 0, direction.y))
                        || (bitMask & 0b100000) > 0 && FindPathFaceDiagonal(current, new Vector3Int(0, direction.y, direction.z))
                        || (bitMask & 0b1) > 0 && FindPathOrthogonal(current, new Vector3Int(direction.x, 0, 0))
                        || (bitMask & 0b10) > 0 && FindPathOrthogonal(current, new Vector3Int(0, direction.y, 0))
                        || (bitMask & 0b100) > 0 && FindPathOrthogonal(current, new Vector3Int(0, 0, direction.z))
                        || !ScanSO.ScanCheck(process, start, current);
                    if (found)
                    {
                        AddToPath(current);
                        break;
                    }
                }
                return found;
            }

            bool FindPathBodyDiagonal_Lazy(Node3D start, Vector3Int direction)
            {
                Node3D current = start;
                Node3D ax, ay, az, axay, axaz, ayaz, next;

                int bitMask = PathFinding3DUtility.BodyDiagonalMoveCheck(process, current, process.ExploreCheck, direction,
                    out next, out ax, out ay, out az, out axay, out axaz, out ayaz);
                if ((bitMask & 0b1000000) == 0)
                    return false;
                current = next;
                current.UpdateParent(start);

                for (; ; )
                {
                    AddToPath(current);
                    if (current == process.To)
                    {
                        ret.Add(current);
                        return true;
                    }

                    bitMask = PathFinding3DUtility.BodyDiagonalMoveCheck(process, current, moveCheck, direction,
                        out next, out ax, out ay, out az, out axay, out axaz, out ayaz);

                    // 6个组成方向（3直线+3面对角线）各挂1步relay，不做完整的递归子扫描——relay自己被Open堆
                    // 弹出时，会按其Parent方向自动继续做完整扫描（正交的走FindPathOrthogonal，面对角线的走FindPathFaceDiagonal）
                    if ((bitMask & 0b1) > 0) { ax.UpdateParent(current); AddToPath(ax); }
                    if ((bitMask & 0b10) > 0) { ay.UpdateParent(current); AddToPath(ay); }
                    if ((bitMask & 0b100) > 0) { az.UpdateParent(current); AddToPath(az); }
                    if ((bitMask & 0b1000) > 0) { axay.UpdateParent(current); AddToPath(axay); }
                    if ((bitMask & 0b10000) > 0) { axaz.UpdateParent(current); AddToPath(axaz); }
                    if ((bitMask & 0b100000) > 0) { ayaz.UpdateParent(current); AddToPath(ayaz); }
                    if ((bitMask & 0b1000000) > 0)
                    {
                        next.UpdateParent(current);
                        AddToPath(next);
                    }
                    current.state = ENodeState.Close;
                    if (GetForcedNeighbourBodyDiagonal(current, direction))
                        break;
                    if ((bitMask & 0b1000000) == 0)
                        break;
                    current = next;
                    if (!ScanSO.ScanCheck(process, start, current))
                        break;
                }
                return true;
            }

            bool GetSingleForcedNeighbour(Node3D current, Vector3Int direction, List<Vector3Int> expectedPass, List<Vector3Int> unexpectedPass)
            {
                Vector3Int pos = current.Position;
                Node3D next = process.PeekNode(pos + direction);
                if (!moveCheck(current, next))
                    return false;

                foreach (Vector3Int v in unexpectedPass)
                {
                    if (PathFinding3DUtility.AdjoinMoveCheck(process, current, v, moveCheck))
                        return false;
                }

                bool passable = false;
                foreach (Vector3Int v in expectedPass)
                {
                    if (PathFinding3DUtility.AdjoinMoveCheck(process, current, v, moveCheck))
                    {
                        passable = true; 
                        break;
                    }
                }
                if (!passable)
                    return false;

                next.UpdateParent(current);
                AddToPath(next);
                return true;
            }

            bool GetForcedNeighbourOrthogonal(Node3D current, Vector3Int enterDirection)
            {
                bool hasFocedNeighbour = false;
                Vector3Int[] directions = PathFinding3DUtility.SortedTwentySixDirections[enterDirection];

                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[1], new() { directions[0] }, new() { directions[9] });
                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[2], new() { directions[0] }, new() { directions[11] });
                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[3], new() { directions[0] }, new() { directions[13] });
                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[4], new() { directions[0] }, new() { directions[15] });

                //TODO:体对角线

                if (hasFocedNeighbour)
                    AddToPath(current);
                return hasFocedNeighbour;
            }

            //与面对角线平行的方向
            bool GetForcedNeighbourFaceDiagonal(Node3D current, Vector3Int enterDirection)
            {
                bool hasFocedNeighbour = false;
                Vector3Int[] directions = PathFinding3DUtility.SortedTwentySixDirections[enterDirection];

                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[9], new() { directions[3] }, new() { directions[21] });
                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[13], new() { directions[4] }, new() { directions[22] });


                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[5], new() { directions[3] }, new() { directions[11], directions[21] });
                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[8], new() { directions[3] }, new() { directions[15], directions[21] });
                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[6], new() { directions[4] }, new() { directions[11], directions[22] });
                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[7], new() { directions[4] }, new() { directions[15], directions[22] });

                //TODO:体对角线

                if (hasFocedNeighbour)
                    AddToPath(current);
                return hasFocedNeighbour;
            }

            //与体对角线平行的方向
            bool GetForcedNeighbourBodyDiagonal(Node3D current, Vector3Int enterDirection)
            {
                bool hasFocedNeighbour = false;
                Vector3Int[] directions = PathFinding3DUtility.SortedTwentySixDirections[enterDirection];

                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[10], new() { directions[4] }, new() { directions[19], directions[22] });
                hasFocedNeighbour |= GetSingleForcedNeighbour(current, directions[13], new() { directions[3] }, new() { directions[20], directions[24] });

                //TODO:体对角线

                if (hasFocedNeighbour)
                    AddToPath(current);

                return hasFocedNeighbour;
            }

            if (from == process.From)
            {
                foreach (Vector3Int direction in PathFinding3DUtility.Orthogonals)
                {
                    FindPathOrthogonal(from, direction);
                }
                foreach (Vector3Int direction in PathFinding3DUtility.FaceDiagonalDirections)
                {
                    FindPathFaceDiagonal(from, direction);
                }
                foreach (Vector3Int direction in PathFinding3DUtility.BodyDiagonalDirections)
                {
                    FindPathBodyDiagonal(from, direction);
                }
                return;
            }

            ret.Clear();

            Vector3Int v = from.Parent == null ? Vector3Int.left : from.Position - from.Parent.Position;
            v = new Vector3Int(Math.Sign(v.x), Math.Sign(v.y), Math.Sign(v.z));

            // 不再依赖SortedTwentySixDirections的下标（26个方向排序后，面/体对角线互为近邻，
            // 会插进2D原本紧邻的下标之间，索引语义在3D里对不上号，参见GetForcedNeighbour*系列的重写），
            // 统一用v自身的分量显式拆出各个子方向，和GetForcedNeighbourFaceDiagonal/BodyDiagonal保持同一套写法
            int diagFlag = Math.Abs(v.x) + Math.Abs(v.y) + Math.Abs(v.z);
            switch (diagFlag)
            {
                case 1:
                    GetForcedNeighbourOrthogonal(from, v);
                    FindPathOrthogonal(from, v);
                    break;
                case 2:
                    SplitFaceDiagonal(v, out Vector3Int a, out Vector3Int b);

                    GetForcedNeighbourFaceDiagonal(from, v);
                    FindPathFaceDiagonal(from, v);
                    FindPathOrthogonal(from, a);
                    FindPathOrthogonal(from, b);
                    break;
                case 3:
                    GetForcedNeighbourBodyDiagonal(from, v);

                    FindPathBodyDiagonal(from, v);
                    FindPathFaceDiagonal(from, new Vector3Int(v.x, v.y, 0));
                    FindPathFaceDiagonal(from, new Vector3Int(v.x, 0, v.z));
                    FindPathFaceDiagonal(from, new Vector3Int(0, v.y, v.z));
                    FindPathOrthogonal(from, new Vector3Int(v.x, 0, 0));
                    FindPathOrthogonal(from, new Vector3Int(0, v.y, 0));
                    FindPathOrthogonal(from, new Vector3Int(0, 0, v.z));
                    break;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
