using System.Collections.Generic;
using UnityEngine;

namespace AStar.Sample
{
    [CreateAssetMenu(fileName = "�����ƶ�", menuName = "AStar/��ȡ���ڽڵ�ķ���/�����ƶ�")]
    public class GetFourSO : GetAdjoinedNodesSO
    {
        public override void GetAdjoinedNodes(PathFindingProcess process, Node node, List<Node> ret)
        {
            PathFindingUtility.GetAdjoinNodes_Four(process, node, ret);
        }
    }
}