using System.Collections.Generic;

namespace AStar.Sample
{
    public class GetEightSO : GetAdjoinedNodesSO
    {
        public override void GetAdjoinedNodes(PathFindingProcess process, PathNode node, List<PathNode> ret)
        {
            PathFindingUtility.GetAdjoinNodes_Eight(process, node, ret);
        }
    }
}