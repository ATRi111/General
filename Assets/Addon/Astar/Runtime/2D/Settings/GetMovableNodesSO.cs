using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.TwoD
{
    [Serializable]
    public abstract class GetMovableNodesSO : ScriptableObject
    {
        public abstract void GetMovableNodes(PathFindingProcess process, Node2D from, Func<Node2D, Node2D, bool> moveCheck, List<Node2D> ret);
    }
}