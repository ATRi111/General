using System;
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

        protected override PathFindingSettings SettingsBase => settings;

        protected override Node2D GenerateNode(Vector2Int position)
        {
            return settings.GenerateNode(this, position);
        }

        protected override void GetMovableNodes(Node from)
        {
            settings.GetAdjoinNodes(this, (Node2D)from, mover.MoveCheck, adjoins);
        }
    }
}
