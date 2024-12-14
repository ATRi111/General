using System.Collections.Generic;
using UnityEngine;

namespace AStar.Sample
{
    [CreateAssetMenu(fileName = "四向移动", menuName = "AStar/获取相邻节点的方法/四向移动")]
    public class GetFourSO : GetAdjoinedNodesSO
    {
        public override void GetAdjoinedNodes(PathFindingProcess process, Node node, List<Node> ret)
        {
            PathFindingUtility.GetAdjoinNodes_Four(process, node, ret);
        }
    }
}