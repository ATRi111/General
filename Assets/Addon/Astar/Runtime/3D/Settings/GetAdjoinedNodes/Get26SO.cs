using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.ThreeD
{
    [CreateAssetMenu(fileName = "二十六向移动", menuName = "AStar3D/获取相邻可达节点的方法/二十六向移动")]
    public class Get26SO : GetMovableNodes3DSO
    {
        public override void GetMovableNodes(PathFinding3DProcess process, Node3D from, Func<Node3D, Node3D, bool> moveCheck, List<Node> ret)
        {
            PathFinding3DUtility.Get26AdjoinNodes(process, from, moveCheck, ret);
        }
    }
}
