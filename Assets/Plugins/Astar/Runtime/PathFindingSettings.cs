using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [Serializable]
    public class PathFindingSettings 
    {
        public Func<PathFindingProcess, float> CalculateWeight;
        public Func<Vector2,Vector2, float> CalculateDistance;
        public Action<PathFindingProcess,PathNode, List<PathNode>> GetAdjoinNodes;
        public Func<PathNode, PathNode, bool> CheckPassable;
        public Func<Vector2, ENodeType> DefineNodeType;
        public int capacity;
        public int maxDepth;


        /// <param name="capacity">节点容量</param>
        /// <param name="maxDepth">最大检索步数</param>
        /// <param name="calculateWeight">确定HCost权重的方法</param>
        /// <param name="calculateDistance">求两点间无障碍距离的方法</param>
        /// <param name="checkPassable">判断两点间能否通行的方法（不考虑是否相邻）</param>
        public PathFindingSettings(int capacity = 1000, int maxDepth = 2000, 
            Func<PathFindingProcess, float> calculateWeight = null,
            Func<Vector2, Vector2, float> calculateDistance = null,
            Action<PathFindingProcess, PathNode, List<PathNode>> getAdjoinNodes = null, 
            Func<PathNode, PathNode, bool> checkPassable = null,
            Func<Vector2, ENodeType> defineNodeType = null)
        {
            this.capacity = capacity;
            this.maxDepth = maxDepth;
            CalculateWeight = calculateWeight ?? PathFindingUtility.CalculateWeight_Default;
            CalculateDistance = calculateDistance ?? PathFindingUtility.ChebyshevDistance;
            GetAdjoinNodes = getAdjoinNodes ?? PathFindingUtility.GetAdjoinNodes_Eight;
            CheckPassable = checkPassable ?? PathFindingUtility.ChechPassable_Default;
            DefineNodeType = defineNodeType ?? PathFindingUtility.DefineNodeType_Default;
            maxDepth = Mathf.Min(capacity * 2 + 2, maxDepth);
        }
    }
}