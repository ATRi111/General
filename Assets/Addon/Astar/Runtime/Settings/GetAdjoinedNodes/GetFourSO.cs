using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.Sample
{
    [CreateAssetMenu(fileName = "�����ƶ�", menuName = "AStar/��ȡ���ڿɴ�ڵ�ķ���/�����ƶ�")]
    public class GetFourSO : GetMovableNodesSO
    {
        public override void GetMovableNodes(PathFindingProcess process, Node from, Func<Node, Node, bool> moveCheck, List<Node> ret)
        {
            PathFindingUtility.GetAdjoinNodes_Four(process, from, moveCheck, ret);
        }
    }
}