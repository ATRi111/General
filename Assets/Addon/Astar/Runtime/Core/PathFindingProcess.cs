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

        /// <summary>
        /// 获取地图上某个位置的节点，并自动确定其节点类型
        /// </summary>
        internal PathNode GetNode(Vector2Int pos)
        {
            if (discoveredNodes.ContainsKey(pos))
                return discoveredNodes[pos];
            PathNode node = new PathNode(this, pos)
            {
                Type = settings.DefineNodeType(pos)
            };
            discoveredNodes.Add(pos, node);
            countOfQuery++;
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
                if (to.Type != ENodeType.Close && settings.MoveCheck(from, to))
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
        /// <summary>
        /// 所有已发现节点
        /// </summary>
        internal readonly Dictionary<Vector2, PathNode> discoveredNodes = new Dictionary<Vector2, PathNode>();

        private readonly List<PathNode> adjoins_original = new List<PathNode>();
        private readonly List<PathNode> adjoins_handled = new List<PathNode>();
        /// <summary>
        /// 待访问节点表
        /// </summary>
        internal Heap<PathNode> open;

        /// <summary>
        /// 当前访问的点
        /// </summary>
        [SerializeField]
        internal PathNode currentNode;
        public PathNode CurrentNode => currentNode;

        internal PathNode nearest;
        /// <summary>
        /// 当前已访问的离终点最近的点
        /// </summary>
        public PathNode Nearest => nearest;
        [SerializeField]
        internal int countOfTestedNode;
        /// <summary>
        /// 测试过的节点数
        /// </summary>
        public int CountOfTestedNode => countOfTestedNode;

        [SerializeField]
        internal int countOfQuery;
        /// <summary>
        /// 查询节点次数
        /// </summary>
        public int CountOfQuery => countOfQuery;

        [SerializeField]
        internal float currentWeight;
        /// <summary>
        /// 寻路的当前一步中,HCost的权重
        /// </summary>
        public float CurrentWeight => currentWeight;

        #endregion

        #region 运行过程
        /// <summary>
        /// 开始寻路
        /// </summary>
        /// <param name="fromPos">起点</param>
        /// <param name="toPos">终点</param>
        /// <param name="ret">接收结果</param>
        public void Start(Vector2Int fromPos, Vector2Int toPos, List<PathNode> ret = null)
        {
            if (fromPos == toPos)
            {
                Debug.LogWarning("起点与终点相同");
                return;
            }

            isRunning = true;
            countOfQuery = 0;
            countOfTestedNode = 0;
            currentWeight = 1f;

            discoveredNodes.Clear();
            open = new Heap<PathNode>(settings.capacity, new Comparer_Cost());
            output = ret;

            To = GetNode(toPos);
            To.Type = ENodeType.Route;

            From = GetNode(fromPos);
            From.Type = ENodeType.Route;
            From.Parent = null;
            From.UpdateHCost(To);

            open.Push(From);
            nearest = From;
        }
        /// <summary>
        /// 立刻完成寻路
        /// </summary>
        public void Compelete()
        {
            for (; ; )
            {
                if (!NextStep())
                    return;
            }
        }
        /// <summary>
        /// 进行一步寻路
        /// </summary>
        public bool NextStep()
        {
            if (!CheckNextStep())
            {
                if (isRunning)
                    Stop();
                return false;
            }

            currentNode = open.Pop();
            currentNode.Type = ENodeType.Close;
            GetAdjoinPassableNodes(currentNode);
            currentWeight = settings.CalculateWeight(this);

            foreach (PathNode node in adjoins_handled)
            {
                switch (node.Type)
                {
                    case ENodeType.Blank:
                        node.UpdateHCost(To);
                        node.Parent = currentNode;
                        node.Type = ENodeType.Open;
                        open.Push(node);
                        break;
                    case ENodeType.Route:
                        node.Parent = currentNode;
                        nearest = node;
                        Stop();
                        return false;
                    case ENodeType.Open:
                        if (node.GCost > currentNode.GCost + currentNode.CalculateGCost(node))
                            node.Parent = currentNode;
                        break;
                }
                if (node.HCost < nearest.HCost)
                    nearest = node;
                countOfTestedNode++;
            }
            return true;
        }
        /// <summary>
        /// 停止寻路并返回结果
        /// </summary>
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
            if (countOfTestedNode > settings.maxDepth)
            {
                Debug.LogWarning("超出步数限制");
                return false;
            }
            if (open.IsEmpty)
            {
                Debug.LogWarning("找不到路径");
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