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
        public int capacity;
        /// <summary>
        /// 最大Closed节点数
        /// </summary>
        public int maxDepth;
        /// <summary>
        /// HCost权重
        /// </summary>
        public float hCostWeight = 1;

        /// <summary>
        /// 获取相邻节点的方法
        /// </summary>
        public Action<PathFindingProcess, AStarNode, List<AStarNode>> GetAdjoinNodes;
        /// <summary>
        /// 计算地图上两点间距离的方法
        /// </summary>
        public Func<Vector2Int, Vector2Int, float> CalculateDistance;
        /// <summary>
        /// 生成新节点的方法
        /// </summary>
        public Func<PathFindingProcess, Vector2Int, AStarNode> GenerateNode;

        public PathFindingSettings(
            Action<PathFindingProcess, AStarNode, List<AStarNode>> GetAdjoinNodes = null,
            Func<Vector2Int, Vector2Int, float> CalculateDistance = null,
            Func<PathFindingProcess, Vector2Int, AStarNode> GenerateNode = null,
            float hCostWeight = 1,
            int capacity = 1000,
            int maxDepth = 2000)
        {
            this.GetAdjoinNodes = GetAdjoinNodes ?? PathFindingUtility.GetAdjoinNodes_Eight;
            this.CalculateDistance = CalculateDistance ?? PathFindingUtility.ChebyshevDistance;
            this.GenerateNode = GenerateNode ?? PathFindingUtility.GenerateNode_Default;
            this.hCostWeight = hCostWeight;
            this.capacity = capacity;
            this.maxDepth = maxDepth;
        }
    }
}