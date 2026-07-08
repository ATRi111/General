using AStar.TwoD;
using System;
using UnityEngine;

namespace AStar.ThreeD
{
    /// <summary>
    /// 均匀3D网格寻路过程
    /// </summary>
    [Serializable]
    public class PathFinding3DProcess : AStar.PathFindingProcess<Vector3Int, Node3D>
    {
        public PathFinding3DSettings settings;

        /// <summary>
        /// 寻路边界（含边界本身），仅在 <see cref="AStar.PathFindingProcess.useBoundary"/> 为 true 时生效
        /// </summary>
        public Vector3Int boundaryMin, boundaryMax;

        protected override PathFindingSettings Settings => settings;

        /// <summary>
        /// 用于持久化保存一个临时节点
        /// </summary>
        protected internal void PersistNode(Node3D node)
        {
            if (!cachedNodes.ContainsKey(node.Position))
                cachedNodes.Add(node.Position, node);
        }

        protected override Node3D GenerateNode(Vector3Int position)
        {
            return settings.GenerateNode(this, position);
        }

        protected override void GetMovableNodes(Node from)
        {
            settings.GetAdjoinNodes(this, (Node3D)from, mover.MoveCheck, movables);
        }

        protected override bool InBoundary(Vector3Int position)
        {
            return position.x >= boundaryMin.x && position.x <= boundaryMax.x
                && position.y >= boundaryMin.y && position.y <= boundaryMax.y
                && position.z >= boundaryMin.z && position.z <= boundaryMax.z;
        }
    }
}
