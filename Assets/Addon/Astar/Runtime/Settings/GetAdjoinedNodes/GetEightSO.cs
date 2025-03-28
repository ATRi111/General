using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "八向移动", menuName = "AStar/获取相邻可达节点的方法/八向移动")]
    public class GetEightSO : GetMovableNodesSO
    {
        public override void GetMovableNodes(PathFindingProcess process, Node from, Func<Node, Node, bool> moveCheck, List<Node> ret)
        {
            PathFindingUtility.GetAdjoinNodes_Eight(process, from, moveCheck, ret);
        }
    }
}