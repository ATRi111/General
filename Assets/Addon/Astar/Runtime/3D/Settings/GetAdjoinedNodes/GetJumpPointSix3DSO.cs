using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.ThreeD
{
    /// <summary>
    /// 六向（无对角线）JPS。转向完全依赖障碍物几何特征触发的"强制邻居"，因此在完全没有障碍物、
    /// 或障碍物是一圈平整无缺口的围墙（不会产生任何强制邻居）的地图上，若终点不恰好落在某个已发现跳点的
    /// 六条坐标轴射线上，理论上就找不到路径——这是该算法本身的固有局限，与"是否用了人为设置的寻路边界"无关：
    /// 人为边界与用Block实际围一圈墙，对搜索的效果必须完全等效（见 <see cref="FindJumpPointOnDirection"/>）
    /// </summary>
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
                foreach (Vector3Int direction in PathFinding3DUtility.SixDirections)
                    TryAdd(FindJumpPointOnDirection(process, from.Position, direction));
                return;
            }

            // from与Parent之间可能是一次跳跃了多格的结果（depthOnDirection>1时很常见），
            // 两点间的位置差不是单位向量，必须按符号归一化，否则下面无论是拿它继续扫描（每次前进的步长就错了），
            // 还是拿它与SixDirections逐个比较排除来向/反向（比较永远不成立），全都会出错，
            // 表现为JPS从第二层扩展开始基本失效、搜索很快就断掉（找不到路径或提前结束）
            Vector3Int delta = from.Position - from.Parent.Position;
            Vector3Int v = new(Math.Sign(delta.x), Math.Sign(delta.y), Math.Sign(delta.z));

            // 沿来向继续前进
            TryAdd(FindJumpPointOnDirection(process, from.Position, v));

            // 强制邻居：某个垂直方向在"来向上紧邻的前一格"被阻挡、但在当前格畅通，说明只能经由当前节点转向该方向。
            // 这里必须用紧邻的前一格（from.Position - v），而不是from.Parent——Parent同样可能是好几格之外的跳点，
            // 用它来做判断在多格跳跃时是错的
            Node3D prevNode = process.PeekNode(from.Position - v);
            if (prevNode != null)
            {
                Vector3Int toDelta = ((Node3D)process.To).Position - from.Position;
                foreach (Vector3Int p in PathFinding3DUtility.SixDirections)
                {
                    if (p == v || p == -v)
                        continue;

                    if (IsAlignedAlong(toDelta, p))
                    {
                        TryAdd(FindJumpPointOnDirection(process, from.Position, p));
                        continue;
                    }

                    bool blockedAtPrev = BlockedAt(process, prevNode, p);
                    bool blockedAtCurrent = BlockedAt(process, from, p);
                    if (blockedAtPrev && !blockedAtCurrent)
                        TryAdd(process.GetNode(from.Position + p));
                }
            }
        }

        /// <summary>
        /// 判断 <paramref name="delta"/>（终点相对当前节点的位置差）是否恰好落在 <paramref name="direction"/>
        /// 这根坐标轴的正方向上——即另外两个分量都为0，且沿该轴的分量与direction同号（同为0则不算对齐，避免二者重合的退化情况）
        /// </summary>
        private static bool IsAlignedAlong(Vector3Int delta, Vector3Int direction)
        {
            if (direction.x != 0)
                return delta.y == 0 && delta.z == 0 && Math.Sign(delta.x) == direction.x;
            if (direction.y != 0)
                return delta.x == 0 && delta.z == 0 && Math.Sign(delta.y) == direction.y;
            return delta.x == 0 && delta.y == 0 && Math.Sign(delta.z) == direction.z;
        }

        /// <summary>
        /// 判断从 <paramref name="at"/> 沿 <paramref name="delta"/> 方向是否被阻挡；
        /// 若该方向上的位置超出寻路边界（<see cref="PathFindingProcess{TPosition,TNode}.PeekNode"/> 返回 null），也视为阻挡
        /// </summary>
        private static bool BlockedAt(PathFinding3DProcess process, Node3D at, Vector3Int delta)
        {
            Node3D peek = process.PeekNode(at.Position + delta);
            return peek == null || !process.mover.MoveCheck(at, peek);
        }

        public static bool PeekTarget(Vector3Int current, Vector3Int to)
        {
            Vector3Int v = to - current;
            return PathFinding3DUtility.Align6(v);
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
                // 越过寻路边界（PeekNode返回null）和撞到真实障碍物（MoveCheck为false）统一处理：
                // 如果给地图直接围一圈Block当边界，走到围墙时同样是MoveCheck失败，效果必须和这里一致，
                // 不应该单独给"越界"开特例去回退到最后一格——沿途没有触发任何强制邻居的话，
                // 这个方向本来就该到此为止，是JPS的正常语义（详见类注释：这是六向JPS在无障碍物地图上的固有局限，不是bug）
                if (probe == null || !process.mover.MoveCheck(prev, probe))
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

                bool blockedAtPrev = BlockedAt(process, prevNode, p);
                bool blockedAtCurrent = BlockedAt(process, node, p);
                if (blockedAtPrev && !blockedAtCurrent)
                    return true;
            }

            return false;
        }
    }
}
