using System;
using System.Diagnostics;
using UnityEngine;

namespace AStar.TwoD
{
    /// <summary>
    /// 2D网格寻路过程
    /// </summary>
    [Serializable]
    public class PathFinding2DProcess : AStar.PathFindingProcess<Vector2Int, Node2D>
    {
        public PathFinding2DSettings settings;

        /// <summary>
        /// 寻路边界（含边界本身），仅在 <see cref="AStar.PathFindingProcess.useBoundary"/> 为 true 时生效
        /// </summary>
        public Vector2Int boundaryMin, boundaryMax;

        protected override PathFindingSettings SettingsBase => settings;

        /// <summary>
        /// 用于持久化保存一个临时节点
        /// </summary>
        protected internal void PersistNode(Node2D node)
        {
            if (!discoveredNodes.ContainsKey(node.Position))
                discoveredNodes.Add(node.Position, node);
        }

        protected override Node2D GenerateNode(Vector2Int position)
        {
            return settings.GenerateNode(this, position);
        }

        protected override void GetMovableNodes(Node from)
        {
            settings.GetAdjoinNodes(this, (Node2D)from, mover.MoveCheck, adjoins);
        }

        protected override bool InBoundary(Vector2Int position)
        {
            return position.x >= boundaryMin.x && position.x <= boundaryMax.x
                && position.y >= boundaryMin.y && position.y <= boundaryMax.y;
        }
    }
}
