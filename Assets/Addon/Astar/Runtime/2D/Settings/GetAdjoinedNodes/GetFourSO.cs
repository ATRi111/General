using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "四向移动", menuName = "AStar/获取相邻可达节点的方法/四向移动")]
    public class GetFourSO : GetMovableNodesSO
    {
        public override void GetMovableNodes(PathFindingProcess process, Node2D from, Func<Node2D, Node2D, bool> moveCheck, List<Node2D> ret)
        {
            PathFinding2DUtility.GetAdjoinNodes_Four(process, from, moveCheck, ret);
        }
    }
}