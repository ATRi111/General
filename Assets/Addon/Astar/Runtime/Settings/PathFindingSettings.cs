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
        public int capacity = 1000;
        /// <summary>
        /// ���Closed�ڵ���
        /// </summary>
        public int maxDepth = 2000;
        /// <summary>
        /// HCostȨ��
        /// </summary>
        public float hCostWeight = 1;

        /// <summary>
        /// ��ȡ���ڽڵ�ķ���
        /// </summary>
        public Action<PathFindingProcess, Node, List<Node>> GetAdjoinNodes;
        public GetAdjoinedNodesSO getAdjoinedNodesSO;

        /// <summary>
        /// ������������ķ���
        /// </summary>
        public Func<Vector2Int, Vector2Int, float> CalculateDistance;
        public CalculateDistanceSO calculateDistanceSO;

        /// <summary>
        /// �����½ڵ�ķ���
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