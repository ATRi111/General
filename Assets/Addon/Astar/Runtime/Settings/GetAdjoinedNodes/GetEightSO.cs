using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "�����ƶ�", menuName = "AStar/��ȡ���ڽڵ�ķ���/�����ƶ�")]
    public class GetEightSO : GetAdjoinedNodesSO
    {
        public override void GetAdjoinedNodes(PathFindingProcess process, Node node, List<Node> ret)
        {
            PathFindingUtility.GetAdjoinNodes_Eight(process, node, ret);
        }
    }
}