using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [Serializable]
    public class PathFindingSettings
    {
        public bool debugMode;
        public int capacity;
        public int maxDepth;

        public Func<PathFindingProcess, float> CalculateWeight;
        public Func<Vector2Int,Vector2Int, float> CalculateDistance;
        public Action<PathFindingProcess,PathNode, List<PathNode>> GetAdjoinNodes;
        public Func<PathNode, PathNode, bool> CheckPassable;
        public Func<Vector2Int, ENodeType> DefineNodeType;

        /// <param name="capacity">�ڵ�����</param>
        /// <param name="maxDepth">����������</param>
        /// <param name="calculateWeight">ȷ��HCostȨ�صķ���</param>
        /// <param name="calculateDistance">����������ϰ�����ķ���</param>
        /// <param name="checkPassable">�ж�������ܷ�ͨ�еķ������������Ƿ����ڣ�</param>
        public PathFindingSettings(bool debugMode = false, int capacity = 1000, int maxDepth = 2000, 
            Func<PathFindingProcess, float> calculateWeight = null,
            Func<Vector2Int, Vector2Int, float> calculateDistance = null,
            Action<PathFindingProcess, PathNode, List<PathNode>> getAdjoinNodes = null, 
            Func<PathNode, PathNode, bool> checkPassable = null,
            Func<Vector2Int, ENodeType> defineNodeType = null)
        {
            this.debugMode = debugMode;
            this.capacity = capacity;
            this.maxDepth = maxDepth;
            CalculateWeight = calculateWeight ?? PathFindingUtility.CalculateWeight_Default;
            CalculateDistance = calculateDistance ?? PathFindingUtility.ChebyshevDistance;
            GetAdjoinNodes = getAdjoinNodes ?? PathFindingUtility.GetAdjoinNodes_Eight;
            CheckPassable = checkPassable ?? PathFindingUtility.CheckPassable_Default;
            DefineNodeType = defineNodeType ?? PathFindingUtility.DefineNodeType_Default;
            maxDepth = Mathf.Min(capacity * 2 + 2, maxDepth);
        }
    }
}