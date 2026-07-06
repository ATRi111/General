using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.ThreeD
{
    [CreateAssetMenu(fileName = "六向跳点", menuName = "AStar3D/获取相邻可达节点的方法/六向跳点")]
    public class GetJumpPointSix3DSO : GetMovableNodes3DSO
    {
        public int depthOnDirection;

        public override void GetMovableNodes(PathFinding3DProcess process, Node3D from, Func<Node3D, Node3D, bool> moveCheck, List<Node> ret)
        {
            ret.Clear();

            void TryAdd(Node3D to)
            {
                if (to != null && moveCheck(from, to))
                    ret.Add(to);
            }

            if (from.Parent == null)
            {
                // 起点没有"来向"可供剪枝参考，六个方向都需要各自尝试寻找跳点
                foreach (Vector3Int direction in PathFinding3DUtility.SixDirections)
                    TryAdd(FindJumpPointOnDirection(process, from.Position, direction));
                return;
            }

            Vector3Int v = from.Position - from.Parent.Position;

            // 沿来向继续前进
            TryAdd(FindJumpPointOnDirection(process, from.Position, v));

            // 强制邻居：某个垂直方向在上一格被阻挡、但在当前格畅通，说明只能经由当前节点转向该方向
            foreach (Vector3Int p in PathFinding3DUtility.SixDirections)
            {
                if (p == v || p == -v)
                    continue;

                bool blockedAtPrev = !process.mover.MoveCheck(from.Parent, process.PeekNode(from.Parent.Position + p));
                bool blockedAtCurrent = !process.mover.MoveCheck(from, process.PeekNode(from.Position + p));
                if (blockedAtPrev && !blockedAtCurrent)
                    TryAdd(process.GetNode(from.Position + p));
            }
        }

        /// <summary>
        /// 沿某个方向扫描寻找跳点。扫描途中经过的中间格子只用于障碍判定（<see cref="PathFindingProcess{TPosition,TNode}.PeekNode"/>，不缓存），
        /// 只有最终确定要返回的位置（跳点本身，或达到搜索深度上限时的兜底位置）才会转换为真正缓存的节点
        /// </summary>
        public Node3D FindJumpPointOnDirection(PathFinding3DProcess process, Vector3Int from, Vector3Int direction)
        {
            Vector3Int current = from;
            Node3D prev = process.PeekNode(from);
            bool moved = false;
            for (int i = 0; i < depthOnDirection; i++)
            {
                current += direction;
                Node3D probe = process.PeekNode(current);
                if (!process.mover.MoveCheck(prev, probe))
                    return null;

                Node3D prevForCheck = prev;
                prev = probe;
                moved = true;

                if (IsJumpPoint(process, probe, prevForCheck, direction))
                    return process.GetNode(current);
            }
            return moved ? process.GetNode(current) : null;
        }

        /// <summary>
        /// 判断 <paramref name="node"/>（沿 <paramref name="direction"/> 方向前进途中经过的格子）是否为跳点。
        /// 六向移动不存在对角线，无法像8向JPS那样只看当前格本身（阻挡的垂直方向+畅通的对角线）来判定，
        /// 因为正交移动下前进和转向的顺序不影响总步数/花费——如果某个垂直方向在"上一格"（<paramref name="prevNode"/>）
        /// 和当前格都畅通，那么从上一格先转向再前进同样能到达，当前格不必也生成这个后继（会与上一格的后继重复）；
        /// 只有当该垂直方向在上一格是阻挡的、但在当前格畅通时，途径当前格才是到达该垂直方向唯一的最短路线，
        /// 此时当前格才算跳点（需要停下来，作为后续节点，以便将来在此处向该方向分支）
        /// </summary>
        private bool IsJumpPoint(PathFinding3DProcess process, Node3D node, Node3D prevNode, Vector3Int direction)
        {
            // node 可能是 PeekNode 探测出的临时节点，与 process.To 比较需按位置而非引用判断
            if (node.Position == ((Node3D)process.To).Position)
                return true;

            foreach (Vector3Int p in PathFinding3DUtility.SixDirections)
            {
                if (p == direction || p == -direction)
                    continue;

                bool blockedAtPrev = !process.mover.MoveCheck(prevNode, process.PeekNode(prevNode.Position + p));
                bool blockedAtCurrent = !process.mover.MoveCheck(node, process.PeekNode(node.Position + p));
                if (blockedAtPrev && !blockedAtCurrent)
                    return true;
            }

            return false;
        }
    }
}
