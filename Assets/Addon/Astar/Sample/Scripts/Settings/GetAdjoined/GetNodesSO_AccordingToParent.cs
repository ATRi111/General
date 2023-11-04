using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "与前进方向更接近的若干节点")]
    public class GetNodesSO_AccordingToParent : GetAdjoinedNodesSO
    {
        public int count;

        public override void GetAdjoinedNodes(PathFindingProcess process, PathNode node, List<PathNode> ret)
        {
            if (node.Parent == null)
            {
                PathFindingUtility.GetAdjoinNodes_Eight(process, node, ret);
                return;
            }
            ret.Clear();
            PathFindingUtility.Comparer_Vector2_Nearer comparer = new PathFindingUtility.Comparer_Vector2_Nearer(node.Position - node.Parent.Position);
            Vector2Int[] directions = PathFindingUtility.eightDirections.ToArray();
            Array.Sort(directions, comparer);

            for (int i = 0; i < count; i++)
            {
                ret.Add(process.GetNode(node.Position + directions[i]));
            }
        }
    }
}