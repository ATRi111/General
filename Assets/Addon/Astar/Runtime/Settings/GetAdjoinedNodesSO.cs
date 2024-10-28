using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public abstract class GetAdjoinedNodesSO : ScriptableObject
    {
        public abstract void GetAdjoinedNodes(PathFindingProcess process, AStarNode node, List<AStarNode> ret);
    }
}