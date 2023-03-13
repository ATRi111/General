using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [Serializable]
    /// <summary>
    /// 一次寻路过程中，某一步的状态
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
        /// 当前访问的点
        /// </summary>
        public PathNode CurrentNode => currentNode;

        internal PathNode nearest;
        /// <summary>
        /// 当前已访问的离终点最近的点
        /// </summary>
        public PathNode Nearest => nearest;
        [SerializeField]
        internal int depth;
        /// <summary>
        /// 搜索步数
        /// </summary>
        public int Depth => depth;

        [SerializeField]
        internal float currentWeight;
        /// <summary>
        /// 寻路的当前一步中,HCost的权重
        /// </summary>
        public float CurrentWeight => currentWeight;

        public PathFindingState()
        {
            currentWeight = 1f;
        }
    }
}