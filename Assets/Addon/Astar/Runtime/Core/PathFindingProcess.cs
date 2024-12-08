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

        public AStarMover mover;
        public MonoBehaviour mountPoint;

        public List<AStarNode> output = new();
        public List<AStarNode> available = new();

        #region 基础方法

        /// <summary>
        /// 获取地图上某个位置的节点，必要时创建新节点
        /// </summary>
        internal AStarNode GetNode(Vector2Int pos)
        {
            if (discoveredNodes.ContainsKey(pos))
                return discoveredNodes[pos];
            AStarNode node = settings.GenerateNode(this, pos);
            discoveredNodes.Add(pos, node);
            countOfQuery++;
            return node;
        }

        /// <summary>
        /// 获取与一个节点相邻的节点(过滤不可通行节点)
        /// </summary>
        internal void GetFilteredAdjoinNodes(AStarNode from)
        {
            adjoins.Clear();
            adjoins_filtered.Clear();
            settings.GetAdjoinNodes.Invoke(this, from, adjoins);
            foreach (AStarNode to in adjoins)
            {
                if (mover.MoveCheck(from, to))
                    adjoins_filtered.Add(to);
            }
        }

        public AStarNode[] GetAllNodes()
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
        private AStarNode from;
        /// <summary>
        /// 起点
        /// </summary>
        public AStarNode From => from;
        [SerializeField]
        private AStarNode to;
        /// <summary>
        /// 终点
        /// </summary>
        public AStarNode To => to;
        /// <summary>
        /// 所有已发现节点
        /// </summary>
        internal readonly Dictionary<Vector2, AStarNode> discoveredNodes = new();

        private readonly List<AStarNode> adjoins = new();
        private readonly List<AStarNode> adjoins_filtered = new();
        /// <summary>
        /// 待访问节点表
        /// </summary>
        internal Heap<AStarNode> open;

        /// <summary>
        /// 当前经过的节点
        /// </summary>
        public AStarNode currentNode;
        /// <summary>
        /// 在可计算的范围内，到终点距离最近的节点
        /// </summary>
        public AStarNode nearest;

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
        /// <summary>
        /// 开始寻路
        /// </summary>
        /// <param name="fromPos">起点</param>
        /// <param name="toPos">终点</param>
        /// <param name="ret">接收结果</param>
        public void Start(Vector2Int fromPos, Vector2Int toPos)
        {
            if (fromPos == toPos)
            {
                Debug.LogWarning("起点与终点相同");
                return;
            }

            isRunning = true;
            countOfQuery = 0;
            countOfCloseNode = 0;

            available.Clear();
            output.Clear();
            discoveredNodes.Clear();
            open.Clear();

            to = GetNode(toPos);
            To.HCost = 0;

            from = GetNode(fromPos);
            From.Parent = null;
            From.HCost = From.CostTo(To);

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
                return false;
            }

            GetFilteredAdjoinNodes(currentNode);

            foreach (AStarNode node in adjoins_filtered)
            {
                switch (node.state)
                {
                    case ENodeState.Blank:
                        node.HCost = node.CostTo(To);
                        node.Parent = currentNode;      //一开始得到的GCost不一定是最小的，故不在这里进行MoveAbilityCheck
                        node.state = ENodeState.Open;
                        open.Push(node);
                        break;
                    case ENodeState.Open:
                        node.UpdateParent(currentNode);
                        break;
                }
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
            if (countOfCloseNode > settings.maxDepth)
            {
                Debug.LogWarning("超出搜索深度限制");
                return false;
            }
            if (open.IsEmpty)
            {
                //Debug.LogWarning("找不到路径");
                return false;
            }
            return true;
        }
        #endregion

        public PathFindingProcess(PathFindingSettings settings, AStarMover mover = null)
        {
            this.settings = settings;
            this.mover = mover ?? new AStarMover();
            open = new Heap<AStarNode>(settings.capacity, new Comparer_Cost());
        }
    }
}