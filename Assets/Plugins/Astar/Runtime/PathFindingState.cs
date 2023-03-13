using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [Serializable]
    /// <summary>
    /// һ��Ѱ·�����У�ĳһ����״̬
    /// </summary>
    public class PathFindingState
    {
        internal readonly Dictionary<Vector2, PathNode> discoveredNodes = new Dictionary<Vector2, PathNode>();

        internal readonly List<PathNode> adjoins_original = new List<PathNode>();
        internal readonly List<PathNode> adjoins_handled = new List<PathNode>();

        internal Heap<PathNode> open;

        [SerializeField]
        internal PathNode currentNode;
        /// <summary>
        /// ��ǰ���ʵĵ�
        /// </summary>
        public PathNode CurrentNode => currentNode;

        internal PathNode nearest;
        /// <summary>
        /// ��ǰ�ѷ��ʵ����յ�����ĵ�
        /// </summary>
        public PathNode Nearest => nearest;
        [SerializeField]
        internal int depth;
        /// <summary>
        /// ��������
        /// </summary>
        public int Depth => depth;

        [SerializeField]
        internal float currentWeight;
        /// <summary>
        /// Ѱ·�ĵ�ǰһ����,HCost��Ȩ��
        /// </summary>
        public float CurrentWeight => currentWeight;

        public PathFindingState()
        {
            currentWeight = 1f;
        }
    }
}