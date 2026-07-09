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
                Vector3Int pos = current.Position;
                bool isJumpPoint = false;
                for (int i = 0; i < depthOnDirection; i++)
                {
                    pos += direction;
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
                // 只依赖direction本身，扫描全程不变，循环外拆一次即可
                SplitFaceDiagonal(direction, out Vector3Int a, out Vector3Int b);

                Node3D current = start;
                Vector3Int pos = current.Position;
                bool isJumpPoint = false;
                for (int i = 0; i < depthOnDirection; i++)
                {
                    pos += direction;
                    Node3D next = process.PeekNode(pos);
                    if (!process.ExploreCheck(current, next))
                        return false;

                    //宽松角规则：a、b两条侧路只要有一条从current出发是畅通的，就允许切这个角
                    Node3D node1 = process.PeekNode(current.Position + a);
                    Node3D node2 = process.PeekNode(current.Position + b);
                    if (!moveCheck(current, node1) && !moveCheck(current, node2))
                        return false;

                    current = next;
                    current.UpdateParent(start);    //必须正确地从前往后设置Parent

                    if (current == process.To)
                    {
                        ret.Add(current);
                        return true;
                    }

                    isJumpPoint = GetForcedNeighbourFaceDiagonal(current, direction)
                        || FindPathOrthogonal(current, a)
                        || FindPathOrthogonal(current, b);
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
                // direction三个分量都非零(体对角线)，拆成三个直线自然分量ax、ay、az，
                // 同样只依赖direction本身，循环外算一次
                Vector3Int ax = new(direction.x, 0, 0);
                Vector3Int ay = new(0, direction.y, 0);
                Vector3Int az = new(0, 0, direction.z);

                Node3D current = start;
                Vector3Int pos = current.Position;
                bool isJumpPoint = false;
                for (int i = 0; i < depthOnDirection; i++)
                {
                    pos += direction;
                    Node3D next = process.PeekNode(pos);
                    if (!process.ExploreCheck(current, next))
                        return false;

                    //体对角线转角比面对角线更宽，仿照Get26AdjoinNodes的宽松角规则：
                    //3个面(经过一个直线分量)+3条棱(经过一个面对角线分量)一共6条侧路，任意一条通畅即可切角
                    Node3D node1 = process.PeekNode(current.Position + ax);
                    Node3D node2 = process.PeekNode(current.Position + ay);
                    Node3D node3 = process.PeekNode(current.Position + az);
                    Node3D node4 = process.PeekNode(current.Position + ax + ay);
                    Node3D node5 = process.PeekNode(current.Position + ax + az);
                    Node3D node6 = process.PeekNode(current.Position + ay + az);
                    bool canCut = moveCheck(current, node1) && moveCheck(node1, next)
                        || moveCheck(current, node2) && moveCheck(node2, next)
                        || moveCheck(current, node3) && moveCheck(node3, next)
                        || moveCheck(current, node4) && moveCheck(node4, next)
                        || moveCheck(current, node5) && moveCheck(node5, next)
                        || moveCheck(current, node6) && moveCheck(node6, next);
                    if (!canCut)
                        return false;

                    current = next;
                    current.UpdateParent(start);    //必须正确地从前往后设置Parent

                    if (current == process.To)
                    {
                        ret.Add(current);
                        return true;
                    }

                    isJumpPoint = GetForcedNeighbourBodyDiagonal(current, direction)
                        || FindPathFaceDiagonal(current, ax + ay)
                        || FindPathFaceDiagonal(current, ax + az)
                        || FindPathFaceDiagonal(current, ay + az)
                        || FindPathOrthogonal(current, ax)
                        || FindPathOrthogonal(current, ay)
                        || FindPathOrthogonal(current, az);
                    if (isJumpPoint)
                        break;
                }
                isJumpPoint |= current != start;    //走到最大距离的点也作为预知对角线上跳点的依据
                if (isJumpPoint)
                    AddToPath(current);
                return isJumpPoint;
            }

            // 在(a,b)所在坐标面内做一次2D对角线强制邻居判定：自然分量a/b其中一侧的直线邻格(node5/node6)被挡、
            // 另一侧的自然分量(node1/node2)仍可从current走到时，翻转被挡的一侧得到强制邻居(node3/node4)——
            // 与2D GetForcedNeighbourDiagonal完全同源的规则，用显式向量而不是SortedTwentySixDirections下标，
            // 因为26个方向排序后，面/体对角线互为近邻会插进2D原本紧邻的下标之间，1~6的下标语义在3D里对不上号；
            // 面对角线(GetForcedNeighbourFaceDiagonal)只需调用一次(a,b就是它自己仅有的两个非零分量)，
            // 体对角线(GetForcedNeighbourBodyDiagonal)在XY/XZ/YZ三个坐标面各调用一次
            bool CheckFaceDiagonal(Node3D current, Vector3Int a, Vector3Int b)
            {
                Node3D node1 = process.PeekNode(current.Position + a);
                Node3D node2 = process.PeekNode(current.Position + b);
                Node3D node3 = process.PeekNode(current.Position + a - b);
                Node3D node4 = process.PeekNode(current.Position - a + b);
                Node3D node5 = process.PeekNode(current.Position - b);
                Node3D node6 = process.PeekNode(current.Position - a);

                bool hasFocedNeighbour = false;
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
                return hasFocedNeighbour;
            }

            bool GetForcedNeighbourOrthogonal(Node3D current, Vector3Int enterDirection)
            {
                Node3D node0 = process.PeekNode(current.Position + enterDirection);
                if (!moveCheck(current, node0))
                    return false;

                // enterDirection只有一个非零分量，另外两条坐标轴各自的正负方向，是4个需要分别检查的"侧向"
                // （2D正交前进只有2个侧向，3D因为多一条维度变成4个），每个侧向对应一对(侧邻格,越过侧邻格到达的对角格)
                Vector3Int perpA, perpB;
                if (enterDirection.x != 0) { perpA = new Vector3Int(0, 1, 0); perpB = new Vector3Int(0, 0, 1); }
                else if (enterDirection.y != 0) { perpA = new Vector3Int(1, 0, 0); perpB = new Vector3Int(0, 0, 1); }
                else { perpA = new Vector3Int(1, 0, 0); perpB = new Vector3Int(0, 1, 0); }

                Node3D node1 = process.PeekNode(current.Position + perpA);
                Node3D node2 = process.PeekNode(node0.Position + perpA);
                Node3D node3 = process.PeekNode(current.Position - perpA);
                Node3D node4 = process.PeekNode(node0.Position - perpA);
                Node3D node5 = process.PeekNode(current.Position + perpB);
                Node3D node6 = process.PeekNode(node0.Position + perpB);
                Node3D node7 = process.PeekNode(current.Position - perpB);
                Node3D node8 = process.PeekNode(node0.Position - perpB);

                bool hasFocedNeighbour = false;
                if (!moveCheck(current, node1) && moveCheck(node0, node2))
                {
                    hasFocedNeighbour = true;
                    node2.UpdateParent(current);
                    AddToPath(node2);
                }
                if (!moveCheck(current, node3) && moveCheck(node0, node4))
                {
                    hasFocedNeighbour = true;
                    node4.UpdateParent(current);
                    AddToPath(node4);
                }
                if (!moveCheck(current, node5) && moveCheck(node0, node6))
                {
                    hasFocedNeighbour = true;
                    node6.UpdateParent(current);
                    AddToPath(node6);
                }
                if (!moveCheck(current, node7) && moveCheck(node0, node8))
                {
                    hasFocedNeighbour = true;
                    node8.UpdateParent(current);
                    AddToPath(node8);
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
                // direction恰好有两个非零分量(面对角线)，拆成两个直线自然分量a、b，交给CheckDiagonalPlane判定
                SplitFaceDiagonal(direction, out Vector3Int a, out Vector3Int b);

                bool hasFocedNeighbour = CheckFaceDiagonal(current, a, b);
                if (hasFocedNeighbour)
                    AddToPath(current);

                return hasFocedNeighbour;
            }

            //与体对角线平行的方向
            bool GetForcedNeighbourBodyDiagonal(Node3D current, Vector3Int direction)
            {
                // 体对角线(dx,dy,dz)按三个坐标面拆成三个面对角线分量(XY/XZ/YZ)，每个分量各自交给
                // CheckDiagonalPlane做一次和面对角线同源的2D强制邻居判定
                //
                // 注意：这三次平面分解只能覆盖"结果仍落在某个坐标面内(第三分量为0)"的强制邻居，
                // 没有覆盖"体对角线本身翻转某一个轴"这一类（比如(1,1,1)翻转y变成(1,-1,1)，第三分量不为0）——
                // 这类强制邻居的触发条件目前还没有严格推导，先不处理，等对照论文图示核实后再补
                Vector3Int ax = new(direction.x, 0, 0);
                Vector3Int ay = new(0, direction.y, 0);
                Vector3Int az = new(0, 0, direction.z);

                bool hasFocedNeighbour = false;
                hasFocedNeighbour |= CheckFaceDiagonal(current, ax, ay);
                hasFocedNeighbour |= CheckFaceDiagonal(current, ax, az);
                hasFocedNeighbour |= CheckFaceDiagonal(current, ay, az);

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
                    {
                        SplitFaceDiagonal(v, out Vector3Int a, out Vector3Int b);

                        GetForcedNeighbourFaceDiagonal(from, v);
                        FindPathFaceDiagonal(from, v);
                        FindPathOrthogonal(from, a);
                        FindPathOrthogonal(from, b);
                        break;
                    }
                case 3:
                    {
                        Vector3Int ax = new(v.x, 0, 0);
                        Vector3Int ay = new(0, v.y, 0);
                        Vector3Int az = new(0, 0, v.z);

                        GetForcedNeighbourBodyDiagonal(from, v);
                        FindPathBodyDiagonal(from, v);
                        FindPathFaceDiagonal(from, ax + ay);
                        FindPathFaceDiagonal(from, ax + az);
                        FindPathFaceDiagonal(from, ay + az);
                        FindPathOrthogonal(from, ax);
                        FindPathOrthogonal(from, ay);
                        FindPathOrthogonal(from, az);
                        break;
                    }
                default:
                    throw new ArgumentException();
            }
        }
    }
}
