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

        protected override PathFindingSettings SettingsBase => settings;

        protected override Node3D GenerateNode(Vector3Int position)
        {
            return settings.GenerateNode(this, position);
        }

        protected override void GetMovableNodes(Node from)
        {
            settings.GetAdjoinNodes(this, (Node3D)from, mover.MoveCheck, adjoins);
        }
    }
}
