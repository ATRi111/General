using System.Collections.Generic;

namespace AStar.Sample
{
    public class GetFourSO : GetAdjoinedNodesSO
    {
        public override void GetAdjoinedNodes(PathFindingProcess process, AStarNode node, List<AStarNode> ret)
        {
            PathFindingUtility.GetAdjoinNodes_Four(process, node, ret);
        }
    }
}