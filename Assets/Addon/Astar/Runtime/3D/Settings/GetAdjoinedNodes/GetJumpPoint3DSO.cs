using AStar.TwoD;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.ThreeD
{
    [CreateAssetMenu(fileName = "跳点", menuName = "AStar3D/获取相邻可达节点的方法/六向跳点")]
    public class GetJumpPoint3DSO : GetMovableNodes3DSO
    {
        public int depthOnDirection;

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

            ret.Clear();

            bool FindPathOrthogonal(Node3D start, Vector3Int direction)
            {
                Node3D current = start;
                Vector3Int pos = current.Position;
                bool isJumpPoint = false;
                for (int i = 0; i < depthOnDirection; i++)
                {
                    pos += direction;
                    Node3D next = process.PeekNode(pos);
                    if (!moveCheck(current, next) || next.state != ENodeState.Blank)    //合理的路径不可能互相穿插
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
                isJumpPoint |= current != start;    //走到最大距离的点也加入后续节点，但不作为预知对角线上跳点的依据
                if (isJumpPoint)
                    AddToPath(current);
                return isJumpPoint;
            }

            bool FindPathFaceDiagonal(Node3D start, Vector3Int direction)
            {
                Node3D current = start;
                Vector3Int pos = current.Position;
                bool isJumpPoint = false;
                for (int i = 0; i < depthOnDirection; i++)
                {
                    pos += direction;
                    Node3D next = process.PeekNode(pos);
                    if (!moveCheck(current, next) || next.state != ENodeState.Blank)     //合理的路径不可能互相穿插
                        return false;

                    Node3D horizontal = process.PeekNode(pos - new Vector3Int(0, direction.y));
                    Node3D vertical = process.PeekNode(pos - new Vector3Int(direction.x, 0));
                    if (!moveCheck(current, horizontal) && !moveCheck(current, vertical))
                        return false;

                    current = next;
                    current.UpdateParent(start);    //必须正确地从前往后设置Parent

                    if (current == process.To)
                    {
                        ret.Add(current);
                        return true;
                    }

                    isJumpPoint = GetForcedNeighbourFaceDiagonal(current, direction)
                        || FindPathOrthogonal(current, new Vector3Int(direction.x, 0))
                        || FindPathOrthogonal(current, new Vector3Int(0, direction.y));
                    if (isJumpPoint)
                        break;
                }
                isJumpPoint |= current != start;    //走到最大距离的点也作为预知对角线上跳点的依据
                if (isJumpPoint)
                    AddToPath(current);
                return isJumpPoint;
            }

            bool FindPathBodyDiagonal(Node3D start, Vector3Int direction)
            {
                return false;
            }

            bool GetForcedNeighbourOrthogonal(Node3D current, Vector3Int enterDirection)
            {
                bool hasFocedNeighbour = false;
                Vector3Int[] directions = PathFinding3DUtility.SortedTwentySixDirections[enterDirection];
                Node3D node0 = process.PeekNode(current.Position + directions[0]);
                if (!moveCheck(current, node0))
                    return false;
                Node3D node1 = process.PeekNode(current.Position + directions[1]);
                Node3D node2 = process.PeekNode(current.Position + directions[2]);
                Node3D node3 = process.PeekNode(current.Position + directions[3]);
                Node3D node4 = process.PeekNode(current.Position + directions[4]);

                if (!moveCheck(current, node3) && moveCheck(node0, node1))
                {
                    hasFocedNeighbour = true;
                    node1.UpdateParent(current);
                    AddToPath(node1);
                }
                if (!moveCheck(current, node4) && moveCheck(node0, node2))
                {
                    hasFocedNeighbour = true;
                    node2.UpdateParent(current);
                    AddToPath(node2);
                }

                if (hasFocedNeighbour)
                {
                    node0.UpdateParent(current);
                    AddToPath(current);
                }

                return hasFocedNeighbour;
            }

            //与面对角线平行的方向
            bool GetForcedNeighbourFaceDiagonal(Node3D current, Vector3Int direction)
            {
                bool hasFocedNeighbour = false;
                Vector3Int[] directions = PathFinding3DUtility.SortedTwentySixDirections[direction];

                Node3D node1 = process.PeekNode(current.Position + directions[1]);
                Node3D node2 = process.PeekNode(current.Position + directions[2]);
                Node3D node3 = process.PeekNode(current.Position + directions[3]);
                Node3D node4 = process.PeekNode(current.Position + directions[4]);
                Node3D node5 = process.PeekNode(current.Position + directions[5]);
                Node3D node6 = process.PeekNode(current.Position + directions[6]);
                if (!moveCheck(current, node5) && moveCheck(current, node1) && moveCheck(node1, node3))
                {
                    hasFocedNeighbour = true;
                    node3.UpdateParent(current);
                    AddToPath(node3);
                }
                if (!moveCheck(current, node6) && moveCheck(current, node2) && moveCheck(node2, node4))
                {
                    hasFocedNeighbour = true;
                    node4.UpdateParent(current);
                    AddToPath(node4);
                }

                if (hasFocedNeighbour)
                {
                    AddToPath(current);
                }

                return hasFocedNeighbour;
            }

            //与体对角线平行的方向
            bool GetForcedNeighbourBodyDiagonal(Node3D current, Vector3Int direction)
            {
                bool hasFocedNeighbour = false;

                // 体对角线(dx,dy,dz)按三个坐标面拆成三个面对角线分量(XY/XZ/YZ)，每个分量各自套用与
                // GetForcedNeighbourFaceDiagonal同样的2D强制邻居规则（某一侧的直线邻格被挡、另一侧的直线分量仍畅通时，
                // 翻转被挡一侧的面对角线即为强制邻居）——用显式向量运算而不是SortedTwentySixDirections的下标，
                // 因为26个方向排序后，面对角线/体对角线自身互为近邻会插进2D里原本紧邻的那几个下标之间，
                // 直接沿用2D的下标语义（1~6）在3D里对不上号
                //
                // 注意：这三次平面分解只能覆盖"结果仍落在某个坐标面内(第三分量为0)"的强制邻居，
                // 没有覆盖"体对角线本身翻转某一个轴"这一类（比如(1,1,1)翻转y变成(1,-1,1)，第三分量不为0）——
                // 这类强制邻居的触发条件目前还没有严格推导，先不处理，等对照论文图示核实后再补
                bool CheckPlane(Vector3Int a, Vector3Int b)
                {
                    Vector3Int perpA = -a;
                    Vector3Int perpB = -b;
                    Vector3Int flipB = a + perpB;    //自然分量a保留，b翻转
                    Vector3Int flipA = perpA + b;    //自然分量b保留，a翻转

                    Node3D natA = process.PeekNode(current.Position + a);
                    Node3D natB = process.PeekNode(current.Position + b);
                    Node3D sideA = process.PeekNode(current.Position + perpA);
                    Node3D sideB = process.PeekNode(current.Position + perpB);
                    Node3D forcedFromB = process.PeekNode(current.Position + flipB);
                    Node3D forcedFromA = process.PeekNode(current.Position + flipA);

                    bool found = false;
                    if (!moveCheck(current, sideB) && moveCheck(current, natA) && moveCheck(natA, forcedFromB))
                    {
                        found = true;
                        forcedFromB.UpdateParent(current);
                        AddToPath(forcedFromB);
                    }
                    if (!moveCheck(current, sideA) && moveCheck(current, natB) && moveCheck(natB, forcedFromA))
                    {
                        found = true;
                        forcedFromA.UpdateParent(current);
                        AddToPath(forcedFromA);
                    }
                    return found;
                }

                Vector3Int ax = new(direction.x, 0, 0);
                Vector3Int ay = new(0, direction.y, 0);
                Vector3Int az = new(0, 0, direction.z);

                hasFocedNeighbour |= CheckPlane(ax, ay);
                hasFocedNeighbour |= CheckPlane(ax, az);
                hasFocedNeighbour |= CheckPlane(ay, az);

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

            Vector3Int v = from.Parent == null ? Vector3Int.left : from.Position - from.Parent.Position;
            v = new Vector3Int(Math.Sign(v.x), Math.Sign(v.y), Math.Sign(v.z));
            Vector3Int[] directions = PathFinding3DUtility.SortedTwentySixDirections[v];

            int diagFlag = Math.Abs(v.x) + Math.Abs(v.y) + Math.Abs(v.z);
            switch(diagFlag)
            {
                case 1:
                    GetForcedNeighbourOrthogonal(from, v);
                    FindPathOrthogonal(from, directions[0]);
                    break;
                case 2:
                    GetForcedNeighbourFaceDiagonal(from, v);
                    FindPathFaceDiagonal(from, directions[0]);
                    FindPathOrthogonal(from, directions[3]);
                    FindPathOrthogonal(from, directions[4]);
                    break;
                case 3:
                    GetForcedNeighbourBodyDiagonal(from, v);

                    FindPathBodyDiagonal(from, directions[0]);
                    FindPathFaceDiagonal(from, directions[1]);
                    FindPathFaceDiagonal(from, directions[2]);
                    FindPathFaceDiagonal(from, directions[3]);
                    FindPathOrthogonal(from, directions[4]);
                    FindPathOrthogonal(from, directions[5]);
                    FindPathOrthogonal(from, directions[6]);
                    break;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
