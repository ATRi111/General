using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar.TwoD
{
    [Serializable]
    public class PathFindingSettings
    {
        /// <summary>
        /// 堆容量
        /// </summary>
        public int capacity = 1000;
        /// <summary>
        /// 最大Closed节点数
        /// </summary>
        public int maxDepth = 2000;
        /// <summary>
        /// HCost权重
        /// </summary>
        public float hCostWeight = 1;

        [SerializeField]
        private GetMovableNodesSO getAdjoinedNodesSO;
        [SerializeField]
        private CalculateDistanceSO calculateDistanceSO;
        [SerializeField]
        private GenerateNodeSO generateNodeSO;

        /// <summary>
        /// 获取相邻可达节点
        /// </summary>
        public void GetAdjoinNodes(PathFindingProcess process, Node2D from, Func<Node2D, Node2D, bool> moveCheck, List<Node2D> adjoins)
        {
            if (getAdjoinedNodesSO != null)
                getAdjoinedNodesSO.GetMovableNodes(process, from, moveCheck, adjoins);
            else
                PathFinding2DUtility.GetAdjoinNodes_Four(process, from, moveCheck, adjoins);
        }

        /// <summary>
        /// 计算两点间距离
        /// </summary>
        public float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return calculateDistanceSO != null
                ? calculateDistanceSO.CalculateDistance(from, to)
                : PathFinding2DUtility.ManhattanDistance(from, to);
        }

        /// <summary>
        /// 生成新节点
        /// </summary>
        public Node2D GenerateNode(PathFindingProcess process, Vector2Int position)
        {
            return generateNodeSO != null
                ? generateNodeSO.GenerateNode(process, position)
                : PathFinding2DUtility.GenerateNode_Default(process, position);
        }
    }
}
