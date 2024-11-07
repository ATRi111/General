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
        /// ���Closed�ڵ���
        /// </summary>
        public int maxDepth;
        /// <summary>
        /// HCostȨ��
        /// </summary>
        public float hCostWeight = 1;

        /// <summary>
        /// ��ȡ���ڽڵ�ķ���
        /// </summary>
        public Action<PathFindingProcess, AStarNode, List<AStarNode>> GetAdjoinNodes;
        /// <summary>
        /// �����ͼ����������ķ���
        /// </summary>
        public Func<Vector2Int, Vector2Int, float> CalculateDistance;
        /// <summary>
        /// �����½ڵ�ķ���
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