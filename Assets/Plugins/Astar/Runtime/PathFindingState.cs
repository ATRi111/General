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

        internal PathNode nearest;
        /// <summary>
        /// ��ǰ��Ϊ������·��ĩ�˵ĵ�
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