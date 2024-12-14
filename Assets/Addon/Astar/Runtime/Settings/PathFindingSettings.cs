using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
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

        /// <summary>
        /// 获取相邻节点的方法
        /// </summary>
        public Action<PathFindingProcess, Node, List<Node>> GetAdjoinNodes;
        public GetAdjoinedNodesSO getAdjoinedNodesSO;

        /// <summary>
        /// 计算两点间距离的方法
        /// </summary>
        public Func<Vector2Int, Vector2Int, float> CalculateDistance;
        public CalculateDistanceSO calculateDistanceSO;

        /// <summary>
        /// 生成新节点的方法
        /// </summary>
        public Func<PathFindingProcess, Vector2Int, Node> GenerateNode;
        public GenerateNodeSO generateNodeSO;

        public void Refresh()
        {
            if (GetAdjoinNodes == null)
            {
                if (getAdjoinedNodesSO != null)
                    GetAdjoinNodes = getAdjoinedNodesSO.GetAdjoinedNodes;
                else
                    GetAdjoinNodes = PathFindingUtility.GetAdjoinNodes_Four;
            }
            if (CalculateDistance == null )
            {
                if(calculateDistanceSO != null)
                    CalculateDistance = calculateDistanceSO.CalculateDistance;
                else
                    CalculateDistance = PathFindingUtility.ManhattanDistance;
            }
            if (GenerateNode == null)
            {
                if (generateNodeSO != null)
                    GenerateNode = generateNodeSO.GenerateNode;
                else
                    GenerateNode = PathFindingUtility.GenerateNode_Default;
            }
        }
    }
}