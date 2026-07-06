using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.TwoD
{
    [CreateAssetMenu(fileName = "八向移动", menuName = "AStar2D/获取相邻可达节点的方法/八向移动")]
    public class GetEightSO : GetMovableNodes2DSO
    {
        public override void GetMovableNodes(PathFinding2DProcess process, Node2D from, Func<Node2D, Node2D, bool> moveCheck, List<Node> ret)
        {
            PathFinding2DUtility.GetAdjoinNodes_Eight(process, from, moveCheck, ret);
        }
    }
}
