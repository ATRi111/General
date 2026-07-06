using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.ThreeD
{
    [Serializable]
    public class PathFinding3DSettings : PathFindingSettings<PathFinding3DProcess, Vector3Int, Node3D>
    {
        protected override void GetAdjoinNodes_Default(PathFinding3DProcess process, Node3D from, Func<Node3D, Node3D, bool> moveCheck, List<Node> adjoins)
        {
            PathFinding3DUtility.GetAdjoinNodes_Six(process, from, moveCheck, adjoins);
        }

        protected override float CalculateDistance_Default(Vector3Int from, Vector3Int to)
        {
            return PathFinding3DUtility.ManhattanDistance(from, to);
        }

        protected override Node3D GenerateNode_Default(PathFinding3DProcess process, Vector3Int position)
        {
            return PathFinding3DUtility.GenerateNode_Default(process, position);
        }
    }
}
