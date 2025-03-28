using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.Sample
{
    [CreateAssetMenu(fileName = "四向移动", menuName = "AStar/获取相邻可达节点的方法/四向移动")]
    public class GetFourSO : GetMovableNodesSO
    {
        public override void GetMovableNodes(PathFindingProcess process, Node from, Func<Node, Node, bool> moveCheck, List<Node> ret)
        {
            PathFindingUtility.GetAdjoinNodes_Four(process, from, moveCheck, ret);
        }
    }
}