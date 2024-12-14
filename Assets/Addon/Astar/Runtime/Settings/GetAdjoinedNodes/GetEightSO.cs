using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "八向移动", menuName = "AStar/获取相邻节点的方法/八向移动")]
    public class GetEightSO : GetAdjoinedNodesSO
    {
        public override void GetAdjoinedNodes(PathFindingProcess process, Node node, List<Node> ret)
        {
            PathFindingUtility.GetAdjoinNodes_Eight(process, node, ret);
        }
    }
}