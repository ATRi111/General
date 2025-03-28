using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace AStar
{
    /// <summary>
    /// 寻路过程
    /// </summary>
    [Serializable]
    public class PathFindingProcess
    {
        public PathFindingSettings settings;

        public MoverBase mover;
        public Transform mountPoint;

        public List<Node> output;
        public List<Node> available;

        #region 基础方法

        /// <summary>
        /// 获取地图上某个位置的节点，必要时创建新节点
        /// </summary>
        internal Node GetNode(Vector2Int pos)
        {
            countOfQuery++;
            if (discoveredNodes.ContainsKey(pos))
                return discoveredNodes[pos];
            Node node = settings.GenerateNode(this, pos);
            discoveredNodes.Add(pos, node);
            return node;
        }

        /// <summary>
        /// 获取与一个节点相邻的可达节点
        /// </summary>
        internal void GetMovableNodes(Node from)
        {
            settings.GetAdjoinNodes.Invoke(this, from, mover.MoveCheck, adjoins);
        }

        public Node[] GetAllNodes()
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

        [SerializeField]
        private Node from;
        /// <summary>
        /// 起点
        /// </summary>
        public Node From => from;
        [SerializeField]
        private Node to;
        /// <summary>
        /// 终点
        /// </summary>
        public Node To => to;
        /// <summary>
        /// 所有已发现节点
        /// </summary>
        internal Dictionary<Vector2, Node> discoveredNodes;

        private List<Node> adjoins;
        /// <summary>
        /// 待访问节点表
        /// </summary>
        internal Heap<Node> open;

        /// <summary>
        /// 当前经过的节点
        /// </summary>
        public Node currentNode;
        /// <summary>
        /// 在可计算的范围内，到终点距离最近的节点
        /// </summary>
        public Node nearest;

        [SerializeField]
        internal int countOfCloseNode;
        public int CountOfCloseNode => countOfCloseNode;

        [SerializeField]
        internal int countOfQuery;
        /// <summary>
        /// 查询节点次数
        /// </summary>
        public int CountOfQuery => countOfQuery;

        public float HCostWeight => settings.hCostWeight;

        #endregion

        #region 运行过程

        public void Initialize()
        {
            settings.Refresh();
            mover ??= new MoverBase();

            discoveredNodes = new();
            adjoins = new();
            output = new();
            available = new();
            open = new Heap<Node>(settings.capacity, new Comparer_Cost());
            countOfQuery = 0;
            countOfCloseNode = 0;
        }

        /// <summary>
        /// 开始寻路
        /// </summary>
        /// <param name="fromPos">起点</param>
        /// <param name="toPos">终点</param>
        /// <param name="ret">接收结果</param>
        public void Start(Vector2Int fromPos, Vector2Int toPos)
        {
            Initialize();
            if (fromPos == toPos)
            {
                Debug.LogWarning("起点与终点相同");
                return;
            }

            isRunning = true;

            to = GetNode(toPos);
            To.HCost = 0;

            from = GetNode(fromPos);
            From.Parent = null;
            From.HCost = From.PredictCostTo(To);

            open.Push(From);
            nearest = From;
        }
        /// <summary>
        /// 立刻完成寻路
        /// </summary>
        public void Complete()
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
            Profiler.BeginSample("Step");
            if (!CheckNextStep())
            {
                if (isRunning)
                    Stop();
                Profiler.EndSample();
                return false;
            }

            currentNode = open.Pop();
            currentNode.state = ENodeState.Close;
            countOfCloseNode++;

            if (mover.MoveAbilityCheck(currentNode) && mover.StayCheck(currentNode))
            {
                available.Add(currentNode);
            }
            if (currentNode.HCost < nearest.HCost)
                nearest = currentNode;

            if (currentNode == To)
            {
                Stop();     //移动力受限的情况下，如果权重系数超过1，有可能在没有找到更短可行路径的情况下提前退出
                Profiler.EndSample();
                return false;
            }

            GetMovableNodes(currentNode);

            foreach (Node node in adjoins)
            {
                switch (node.state)
                {
                    case ENodeState.Blank:
                        node.HCost = node.PredictCostTo(To);
                        node.Parent = currentNode;
                        node.state = ENodeState.Open;
                        open.Push(node);
                        break;
                    case ENodeState.Open:
                        node.UpdateParent(currentNode);
                        break;
                }
            }

            Profiler.EndSample();
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
            if (countOfCloseNode > settings.maxDepth)
            {
                Debug.LogWarning($"超出搜索深度限制,最大深度为{settings.maxDepth}");
                return false;
            }
            if (open.IsEmpty)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}