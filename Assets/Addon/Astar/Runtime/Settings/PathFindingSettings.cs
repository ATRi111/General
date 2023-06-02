using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [Serializable]
    public class PathFindingSettings
    {
        /// <summary>
        /// ������
        /// </summary>
        public int capacity;
        /// <summary>
        /// �����Խڵ���
        /// </summary>
        public int maxDepth;

        /// <summary>
        /// �涨HCostȨ�صķ���
        /// </summary>
        public Func<PathFindingProcess, float> CalculateWeight;
        /// <summary>
        /// ����HCost�ķ���
        /// </summary>
        public Func<Vector2Int, Vector2Int, float> CalculateHCost;
        /// <summary>
        /// ����GCost�ķ���
        /// </summary>
        public Func<Vector2Int, Vector2Int, float> CalculateGCost;
        /// <summary>
        /// ��ȡ���ڽڵ�ķ���
        /// </summary>
        public Action<PathFindingProcess, PathNode, List<PathNode>> GetAdjoinNodes;
        /// <summary>
        /// �ж��ܷ��ƶ��ķ���
        /// </summary>
        public Func<PathNode, PathNode, bool> MoveCheck;
        /// <summary>
        /// ȷ���ڵ����͵ķ���
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