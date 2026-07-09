using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// 寻路过程的公共配置，不涉及具体的空间位置表示（2D网格 / 稀疏八叉树等）
    /// </summary>
    [Serializable]
    public class PathFindingSettings
    {
        /// <summary>
        /// HCost权重
        /// </summary>
        public float hCostWeight = 1;
        /// <summary>
        /// 堆容量
        /// </summary>
        public int heapCapacity = 1000;
        /// <summary>
        /// 最大持久存储节点数
        /// </summary>
        public int cacheCapacity = 2000;
        /// <summary>
        /// 临时缓存节点数
        /// </summary>
        public int temporaryCacheCapacity = 100;
    }

    /// <summary>
    /// 携带具体寻路过程/位置/节点类型的寻路配置模板，持有可替换的三个策略 SO，
    /// 未指定策略时回退到由具体空间表示提供的默认实现
    /// </summary>
    [Serializable]
    public abstract class PathFindingSettings<TProcess, TPosition, TNode> : PathFindingSettings
        where TProcess : PathFindingProcess
        where TNode : Node
    {
        [SerializeField]
        private GetMovableNodesSO<TProcess, TNode> getAdjoinedNodesSO;
        [SerializeField]
        private CalculateDistanceSO<TPosition> calculateDistanceSO;
        [SerializeField]
        private GenerateNodeSO<TProcess, TPosition, TNode> generateNodeSO;

        /// <summary>
        /// 当前用于获取相邻可达节点的策略SO名称，供日志/统计展示（如朴素26向、跳点JPS等）；
        /// 未指定SO（走 <see cref="GetAdjoinNodes_Default"/> 兜底）时返回"默认"
        /// </summary>
        public string GetAdjoinedNodesSOName => getAdjoinedNodesSO != null ? getAdjoinedNodesSO.name : "默认";

        /// <summary>
        /// 获取相邻可达节点;必定确保moveCheck包含对to参数的null检查
        /// </summary>
        public void GetAdjoinNodes(TProcess process, TNode from, Func<TNode, TNode, bool> moveCheck, List<Node> adjoins)
        {
            if (getAdjoinedNodesSO != null)
                getAdjoinedNodesSO.GetMovableNodes(process, from, moveCheck, adjoins);
            else
                GetAdjoinNodes_Default(process, from, moveCheck, adjoins);
        }

        /// <summary>
        /// 计算两点间距离
        /// </summary>
        public float CalculateDistance(TPosition from, TPosition to)
        {
            return calculateDistanceSO != null
                ? calculateDistanceSO.CalculateDistance(from, to)
                : CalculateDistance_Default(from, to);
        }

        /// <summary>
        /// 生成新节点
        /// </summary>
        public TNode GenerateNode(TProcess process, TPosition position)
        {
            return generateNodeSO != null
                ? generateNodeSO.GenerateNode(process, position)
                : GenerateNode_Default(process, position);
        }

        /// <summary>
        /// 未指定 <see cref="getAdjoinedNodesSO"/> 时的默认相邻节点获取方式，由具体空间表示实现
        /// </summary>
        protected abstract void GetAdjoinNodes_Default(TProcess process, TNode from, Func<TNode, TNode, bool> moveCheck, List<Node> adjoins);

        /// <summary>
        /// 未指定 <see cref="calculateDistanceSO"/> 时的默认距离计算方式，由具体空间表示实现
        /// </summary>
        protected abstract float CalculateDistance_Default(TPosition from, TPosition to);

        /// <summary>
        /// 未指定 <see cref="generateNodeSO"/> 时的默认节点生成方式，由具体空间表示实现
        /// </summary>
        protected abstract TNode GenerateNode_Default(TProcess process, TPosition position);
    }
}
