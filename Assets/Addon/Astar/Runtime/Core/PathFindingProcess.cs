using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Profiling;

namespace AStar
{
    /// <summary>
    /// 寻路过程的公共部分，不涉及具体的空间位置表示（2D网格 / 稀疏八叉树等）
    /// </summary>
    [Serializable]
    public abstract class PathFindingProcess
    {
        public MoverBase mover;
        public Transform mountPoint;

        /// <summary>
        /// 是否启用寻路边界。启用后，新发现的节点在加入 Open 之前会先经过 <see cref="InBoundary"/> 检查，
        /// 超出边界的节点不会被加入 Open（但仍保持 <see cref="ENodeState.Blank"/>，不会被错误地当作已探索过）
        /// </summary>
        public bool useBoundary;

        /// <summary>
        /// 供基类访问的配置（具体类型由子类决定）
        /// </summary>
        protected abstract PathFindingSettings Settings { get; }
        public float HCostWeight => Settings.hCostWeight;

        [SerializeReference]
        public List<Node> output;
        [SerializeReference]
        public List<Node> available;

        [SerializeField]
        protected bool isRunning;
        /// <summary>
        /// 是否正在进行寻路
        /// </summary>
        public bool IsRunning => isRunning;

        [SerializeReference]
        protected Node from;
        /// <summary>
        /// 起点
        /// </summary>
        public Node From => from;
        [SerializeReference]
        protected Node to;
        /// <summary>
        /// 终点
        /// </summary>
        public Node To => to;

        /// <summary>
        /// 可达节点的临时容器
        /// </summary>
        protected List<Node> movables;

        protected Heap<Node> open;

        /// <summary>
        /// 当前经过的节点
        /// </summary>
        [SerializeReference]
        public Node currentNode;
        /// <summary>
        /// 在可计算的范围内，到终点距离最近的节点
        /// </summary>
        [SerializeReference]
        public Node nearest;

        [SerializeField]
        protected internal int generateCount;

        [SerializeField]
        protected internal int queryCount;

        [SerializeField]
        protected internal int openCount;

        public virtual void Initialize()
        {
            mover ??= new MoverBase();
            movables = new();
            output = new();
            available = new();
            open = new Heap<Node>(Settings.heapCapacity, new Comparer_Cost());
            queryCount = 0;
            generateCount = 0;
        }

        /// <summary>
        /// 获取与一个节点相邻的可达节点，写入 <see cref="movables"/>，由具体空间表示实现
        /// </summary>
        protected abstract void GetMovableNodes(Node from);

        /// <summary>
        /// 开始寻路（要求 <paramref name="from"/>/<paramref name="to"/> 已通过 <see cref="Initialize"/> 后由具体空间表示解析得到）
        /// </summary>
        public virtual void Start(Node from, Node to)
        {
            if (from == to)
            {
                Debug.LogWarning("起点与终点相同");
                return;
            }

            isRunning = true;

            this.to = to;
            to.HCost = 0;

            this.from = from;
            from.HCost = from.PredictCostTo(to);

            open.Push(from);
            from.state = ENodeState.Open;
            nearest = from;
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
        public virtual bool NextStep()
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

            GetMovableNodes(currentNode);       //JPS中，可能会获取到一些非直接相邻和非Blank节点
            if (mover.MoveAbilityCheck(currentNode) && mover.StayCheck(currentNode))
            {
                available.Add(currentNode);
            }

            foreach (Node node in movables)
            {
                node.Parent ??= currentNode;
                if(node.HCost < 0)
                    node.HCost = node.PredictCostTo(to);

                if (node.HCost < nearest.HCost)
                    nearest = node;
                //为了可视化Open节点，这里不更新available

                if (node == to)
                {
                    if (mover.MoveAbilityCheck(to) && mover.StayCheck(to))
                        available.Add(to);
                    Stop();     //如果权重系数超过1，有可能在没有找到更短可行路径的情况下提前退出
                    Profiler.EndSample();
                    return false;
                }
                switch (node.state)
                {
                    case ENodeState.Blank:
                        node.state = ENodeState.Open;
                        open.Push(node);
                        openCount++;
                        break;
                    case ENodeState.Open:
                        //节点的值改变，理论上在堆中的位置需要改变，但目前忽略
                        node.UpdateParent(currentNode);
                        break;
                }
            }

            currentNode.state = ENodeState.Close;
            ClearTemporary();
            Profiler.EndSample();
            return true;
        }

        protected virtual void ClearTemporary()
        {

        }

        /// <summary>
        /// 停止寻路并返回结果
        /// </summary>
        public virtual void Stop()
        {
            from.Parent = null; //寻路过程中，起点的Parent可能被修改
            isRunning = false;
            nearest.Recall(n => output.Add(n));
        }

        protected virtual bool CheckNextStep()
        {
            if (!isRunning)
            {
                Debug.LogWarning("寻路未开始");
                return false;
            }
            if (open.IsEmpty)
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 携带具体位置类型与节点类型的寻路过程模板，适用于位置可直接作为字典 Key 的空间表示（如均匀网格）。
    /// 稀疏空间（如八叉树）若节点身份不能用坐标直接表达，不适用此模板，应直接继承 <see cref="PathFindingProcess"/>。
    /// </summary>
    [Serializable]
    public abstract class PathFindingProcess<TPosition, TNode> : PathFindingProcess
        where TNode : Node
    {
        /// <summary>
        /// 寻路过程中持久存储的节点
        /// </summary>
        protected internal Dictionary<TPosition, TNode> cachedNodes;
        /// <summary>
        /// 寻路过程中临时存储的节点
        /// </summary>
        protected internal Dictionary<TPosition, TNode> temporaryNodes;

        /// <summary>
        /// 生成指定位置的新节点，由具体空间表示实现
        /// </summary>
        protected abstract TNode GenerateNode(TPosition position);

        /// <summary>
        /// 判断某个位置是否在寻路边界内，由具体空间表示实现。仅在 <see cref="PathFindingProcess.useBoundary"/> 为 true 时生效，
        /// 在 <see cref="GetNode"/>/<see cref="PeekNode"/> 生成新节点之前检查（对已缓存过的位置不会重复检查），
        /// 超出边界的位置直接返回 null，调用处需要自行处理
        /// </summary>
        protected abstract bool InBoundary(TPosition position);

        /// <summary>
        /// 获取地图上某个位置的节点，必要时创建新节点；若启用了边界且该位置超出边界，返回 null
        /// </summary>
        internal TNode GetNode(TPosition pos)
        {
            queryCount++;
            if (cachedNodes.ContainsKey(pos))
                return cachedNodes[pos];
            if (useBoundary && !InBoundary(pos))
                return null;
            TNode node = GenerateNode(pos);
            generateCount++;
            cachedNodes.Add(pos, node);
            return node;
        }

        /// <summary>
        /// 临时获取地图上某个位置的节点，不将其加入缓存中，以节约内存，常用于JPS；
        /// 若启用了边界且该位置超出边界，返回 null
        /// </summary>
        internal TNode PeekNode(TPosition pos)
        {
            queryCount++;
            if (cachedNodes.ContainsKey(pos))
                return cachedNodes[pos];
            if (temporaryNodes.ContainsKey(pos))
                return temporaryNodes[pos];
            if (useBoundary && !InBoundary(pos))
                return null;
            TNode node = GenerateNode(pos);
            generateCount++;
            temporaryNodes.Add(pos, node);
            return node;
        }

        public TNode[] GetAllNodes()
        {
            return cachedNodes.Values.ToArray();
        }

        public override void Initialize()
        {
            base.Initialize();
            cachedNodes = new();
            temporaryNodes = new();
        }

        /// <summary>
        /// 开始寻路
        /// </summary>
        /// <param name="fromPos">起点</param>
        /// <param name="toPos">终点</param>
        public void Start(TPosition fromPos, TPosition toPos)
        {
            Initialize();
            TNode to = GetNode(toPos);
            TNode from = GetNode(fromPos);
            if (to == null || from == null)
            {
                Debug.LogWarning("起点或终点超出寻路边界");
                return;
            }
            Start(from, to);
        }

        protected override void ClearTemporary()
        {
            base.ClearTemporary();
            if(temporaryNodes.Count> Settings.temporaryCacheCapacity)
                temporaryNodes.Clear();
        }

        protected override bool CheckNextStep()
        {
            if (!base.CheckNextStep())
                return false;

            if (cachedNodes.Count > Settings.cacheCapacity)
            {
                Debug.LogWarning($"节点缓存容量超出限制");
                return false;
            }
            return true;
        }
    }
}
