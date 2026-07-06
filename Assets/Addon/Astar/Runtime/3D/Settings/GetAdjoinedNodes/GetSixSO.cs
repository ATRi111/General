using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.ThreeD
{
    [CreateAssetMenu(fileName = "六向移动", menuName = "AStar3D/获取相邻可达节点的方法/六向移动")]
    public class GetSixSO : GetMovableNodes3DSO
    {
        public override void GetMovableNodes(PathFinding3DProcess process, Node3D from, Func<Node3D, Node3D, bool> moveCheck, List<Node> ret)
        {
            PathFinding3DUtility.GetAdjoinNodes_Six(process, from, moveCheck, ret);
        }
    }
}
