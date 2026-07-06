using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.TwoD
{
    [Serializable]
    public class PathFinding2DSettings : PathFindingSettings<PathFinding2DProcess, Vector2Int, Node2D>
    {
        protected override void GetAdjoinNodes_Default(PathFinding2DProcess process, Node2D from, Func<Node2D, Node2D, bool> moveCheck, List<Node> adjoins)
        {
            PathFinding2DUtility.GetAdjoinNodes_Four(process, from, moveCheck, adjoins);
        }

        protected override float CalculateDistance_Default(Vector2Int from, Vector2Int to)
        {
            return PathFinding2DUtility.ManhattanDistance(from, to);
        }

        protected override Node2D GenerateNode_Default(PathFinding2DProcess process, Vector2Int position)
        {
            return PathFinding2DUtility.GenerateNode_Default(process, position);
        }
    }
}
