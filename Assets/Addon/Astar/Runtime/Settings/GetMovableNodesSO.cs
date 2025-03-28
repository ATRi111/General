using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [Serializable]
    public abstract class GetMovableNodesSO : ScriptableObject
    {
        public abstract void GetMovableNodes(PathFindingProcess process, Node from, Func<Node, Node, bool> moveCheck, List<Node> ret);
    }
}