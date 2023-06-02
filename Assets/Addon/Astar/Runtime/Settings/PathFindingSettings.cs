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
        /// 最大测试节点数
        /// </summary>
        public int maxDepth;

        /// <summary>
        /// 规定HCost权重的方法
        /// </summary>
        public Func<PathFindingProcess, float> CalculateWeight;
        /// <summary>
        /// 计算HCost的方法
        /// </summary>
        public Func<Vector2Int, Vector2Int, float> CalculateHCost;
        /// <summary>
        /// 计算GCost的方法
        /// </summary>
        public Func<Vector2Int, Vector2Int, float> CalculateGCost;
        /// <summary>
        /// 获取相邻节点的方法
        /// </summary>
        public Action<PathFindingProcess, PathNode, List<PathNode>> GetAdjoinNodes;
        /// <summary>
        /// 判断能否移动的方法
        /// </summary>
        public Func<PathNode, PathNode, bool> MoveCheck;
        /// <summary>
        /// 确定节点类型的方法
        /// </summary>
        public Func<Vector2Int, ENodeType> DefineNodeType;

        public PathFindingSettings(int capacity = 1000, int maxDepth = 2000,
            Func<PathFindingProcess, float> calculateWeight = null,
            Func<Vector2Int, Vector2Int, float> calculateHCost = null,
            Func<Vector2Int, Vector2Int, float> calculateGCost = null,
            Action<PathFindingProcess, PathNode, List<PathNode>> getAdjoinNodes = null,
            Func<PathNode, PathNode, bool> checkPassable = null,
            Func<Vector2Int, ENodeType> defineNodeType = null)
        {
            this.capacity = capacity;
            this.maxDepth = maxDepth;
            CalculateWeight = calculateWeight ?? PathFindingUtility.CalculateWeight_Default;
            CalculateHCost = calculateHCost ?? PathFindingUtility.ChebyshevDistance;
            CalculateGCost = calculateGCost ?? PathFindingUtility.ChebyshevDistance;
            GetAdjoinNodes = getAdjoinNodes ?? PathFindingUtility.GetAdjoinNodes_Eight;
            MoveCheck = checkPassable ?? PathFindingUtility.CheckPassable_Default;
            DefineNodeType = defineNodeType ?? PathFindingUtility.DefineNodeType_Default;
            maxDepth = Mathf.Min(capacity * 2 + 2, maxDepth);
        }
    }
}