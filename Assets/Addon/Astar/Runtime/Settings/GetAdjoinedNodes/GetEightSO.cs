using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "�����ƶ�", menuName = "AStar/��ȡ���ڿɴ�ڵ�ķ���/�����ƶ�")]
    public class GetEightSO : GetMovableNodesSO
    {
        public override void GetMovableNodes(PathFindingProcess process, Node from, Func<Node, Node, bool> moveCheck, List<Node> ret)
        {
            PathFindingUtility.GetAdjoinNodes_Eight(process, from, moveCheck, ret);
        }
    }
}