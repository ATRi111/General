using System;
using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    /// <summary>
    /// 寻路节点的公共基类，不涉及具体的空间位置表示（2D网格 / 稀疏八叉树等）
    /// </summary>
    [System.Serializable]
    public abstract class Node
    {
        [SerializeField]
        public ENodeState state;

        public virtual bool IsObstacle => false;

        /// <summary>
        /// 起点到该点的距离（路径已确定）
        /// </summary>
        public float GCost;

        /// <summary>
        /// 该点到终点的距离（假设无障碍）
        /// </summary>
        public float HCost;

        public abstract float WeightedFCost { get; }
        public float FCost => HCost + GCost;

        private Node parent;
        public Node Parent
        {
            get => parent;
            set
            {
                parent = value;
                GCost = value == null ? 0 : parent.GCost + value.CostTo(this);
            }
        }

        protected internal abstract MoverBase Mover { get; }

        protected Node()
        {
            state = ENodeState.Blank;
            HCost = -1;
        }

        /// <summary>
        /// 当前节点能直接到达到目标节点时,计算两点间距离
        /// </summary>
        public virtual float CostTo(Node to)
        {
            float primitiveCost = PrimitiveCostTo(to);
            return Mover.CalculateCost(this, to, primitiveCost);
        }
        /// <summary>
        /// 当前节点不能直接到达目标节点时,预测两点间距离
        /// </summary>
        public virtual float PredictCostTo(Node to)
        {
            float primitiveCost = PrimitiveCostTo(to);
            return Mover.CalculateCost(this, to, primitiveCost);
        }

        /// <summary>
        /// 计算与另一节点间的原始距离（不考虑移动规则），由具体空间表示实现
        /// </summary>
        protected internal abstract float PrimitiveCostTo(Node to);

        public void UpdateParent(Node node)
        {
            float g = node.GCost + node.CostTo(this);
            if (GCost > g)
                Parent = node;
        }

        /// <summary>
        /// 回溯路径（会考虑移动力，但不会考虑能否停留）
        /// </summary>
        /// <param name="onNode">对回溯路径上依次经过的节点（从起点到当前节点）执行的操作</param>
        public void Recall(Action<Node> onNode = null)
        {
            if (onNode == null)
                return;
            List<Node> stack = new();
            for (Node n = this; n != null; n = n.Parent)
                if (n.Mover.MoveAbilityCheck(n))
                    stack.Add(n);
            for (int i = stack.Count - 1; i >= 0; i--)
                onNode(stack[i]);
        }
    }

    public class Comparer_Cost : IComparer<Node>
    {
        public int Compare(Node x, Node y)
        {
            int c = x.WeightedFCost.CompareTo(y.WeightedFCost);
            if (c != 0)
                return c;
            return y.HCost.CompareTo(x.HCost);  //FCost相等时，优先选择HCost更小的
        }
    }
}
