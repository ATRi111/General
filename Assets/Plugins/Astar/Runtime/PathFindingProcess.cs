using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// 一次寻路过程
    /// </summary>
    [Serializable]
    public class PathFindingProcess
    {
        [SerializeField]
        private PathFindingSettings settings;
        public PathFindingSettings Settings => settings;

        private List<PathNode> output;

        #region 基础方法
        //节点在被访问时才会被创建
        internal PathNode GetNode(Vector2Int pos)
        {
            if (discoveredNodes.ContainsKey(pos))
                return discoveredNodes[pos];
            PathNode node = new PathNode(this, pos)
            {
                Type = settings.DefineNodeType(pos)
            };
            discoveredNodes.Add(pos, node);
            return node;
        }

        /// <summary>
        /// 获取与一个节点相邻且可通行且不为Close的节点
        /// </summary>
        internal void GetAdjoinPassableNodes(PathNode from)
        {
            adjoins_original.Clear();
            adjoins_handled.Clear();
            settings.GetAdjoinNodes.Invoke(this, from, adjoins_original);
            foreach (PathNode to in adjoins_original)
            {
                if (to.Type != ENodeType.Close && settings.CheckPassable(from, to))
                    adjoins_handled.Add(to);
            }
        }

        public PathNode[] GetAllNodes()
        {
            return discoveredNodes.Values.ToArray();
        }
        #endregion

        #region 状态量
        [SerializeField]
        private bool isRunning;
        /// <summary>
        /// 是否正在进行寻路
        /// </summary>
        public bool IsRunning => isRunning;

        /// <summary>
        /// 起点
        /// </summary>
        public PathNode From { get; private set; }
        /// <summary>
        /// 终点
        /// </summary>
        public PathNode To { get; private set; }

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

        #endregion

        #region 运行过程
        public void Start(Vector2Int fromPos, Vector2Int toPos, List<PathNode> ret = null)
        {
            isRunning = true;
            depth = 0;
            currentWeight = 1f;

            discoveredNodes.Clear();
            open = new Heap<PathNode>(settings.capacity, new Comparer_Cost());
            output = ret;
            
            To = GetNode(toPos);
            To.Type = ENodeType.Route;

            From = GetNode(fromPos);
            From.Type = ENodeType.Route;
            From.Parent = null;
            From.CalculateHCost(To);

            open.Push(From);
            nearest = From;
        }

        public void NextStep()
        {
            if (!CheckNextStep())
                return;

            currentNode = open.Pop();
            currentNode.Type = ENodeType.Close;
            GetAdjoinPassableNodes(currentNode);
            currentWeight = settings.CalculateWeight(this);

            
            foreach (PathNode node in adjoins_handled)
            {
                switch (node.Type)
                {
                    case ENodeType.Blank:
                        node.CalculateHCost(To);
                        node.Parent = currentNode;
                        node.Type = ENodeType.Open;
                        open.Push(node);
                        break;
                    case ENodeType.Route:
                        node.Parent = currentNode;
                        nearest = node;
                        Stop();
                        break;
                    case ENodeType.Open:
                        if (node.GCost > currentNode.GCost + currentNode.CalculateDistance(node))
                            node.Parent = currentNode;
                        break;
                }
                if (node.HCost < nearest.HCost)
                    nearest = node;
                depth++;
            }
        }
        public void LastStep()
        {
            if (settings.debugMode == false)
                return;
        }
        public void Stop()
        {
            isRunning = false;
            nearest.Recall(output);
        }
        private bool CheckNextStep()
        {
            if (!isRunning)
            {
                Debug.LogWarning("寻路未开始");
                return false;
            }
            if (depth > settings.maxDepth)
            {
                Debug.LogWarning("超出步数限制");
                Stop();
                return false;
            }
            if (open.IsEmpty)
            {
                Debug.LogWarning("找不到路径");
                Stop();
                return false;
            }
            return true;
        }
        #endregion

        public PathFindingProcess(PathFindingSettings settings)
        {
            this.settings = settings;
        }
    }
}