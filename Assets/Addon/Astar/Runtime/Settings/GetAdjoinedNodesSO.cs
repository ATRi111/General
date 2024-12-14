using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [System.Serializable]
    public class GetAdjoinedNodesSO : ScriptableObject
    {
        public virtual void GetAdjoinedNodes(PathFindingProcess process, Node node, List<Node> ret)
        {

        }
    }
}