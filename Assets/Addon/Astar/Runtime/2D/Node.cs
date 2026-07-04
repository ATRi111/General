using System.Collections.Generic;
using UnityEngine;
using AStar;

namespace AStar.TwoD
{
    [System.Serializable]
    public class Node
    {
        protected internal readonly PathFindingProcess process;
        [SerializeField]
        protected Vector2Int position;
        public Vector2Int Position => position;

        [SerializeField]
        public ENodeState state;

        public virtual bool IsObstacle
        {
            get => false;
        }

        /// <summary>
        /// 起点到该点的距离（路径已确定）
        /// </summary>
        public float GCost;

        /// <summary>
        /// 该点到终点的距离（假设无障碍）
        /// </summary>
        public float HCost;

        public float WeightedFCost => process.HCostWeight * HCost + GCost;
        public float FCost => HCost + GCost;

        private Node parent;
        public Node Parent
        {
            get => parent;
            set
            {
                parent = value;
                GCost = value == null ? 0 : Parent.GCost + value.CostTo(this);
            }
        }

        public Node(PathFindingProcess process, Vector2Int position)
        {
            this.process = process;
            this.position = position;
            state = ENodeState.Blank;
        }
        /// <summary>
        /// 当前节点能直接到达到目标节点时,计算两点间距离
        /// </summary>
        public virtual float CostTo(Node to)
        {
            float primitiveCost = PrimitiveCostTo(to);
            return process.mover.CalculateCost(this, to, primitiveCost);
        }
        /// <summary>
        /// 当前节点不能直接到达目标节点时,预测两点间距离
        /// </summary>
        public virtual float PredictCostTo(Node to)
        {
            float primitiveCost = PrimitiveCostTo(to);
            return process.mover.CalculateCost(this, to, primitiveCost);
        }
        protected internal virtual float PrimitiveCostTo(Node to)
        {
            float distance = process.settings.CalculateDistance(Position, to.Position);
            return distance;
        }
        public void UpdateParent(Node node)
        {
            float g = node.GCost + node.CostTo(this);
            if (GCost > g)
                Parent = node;
        }

        /// <summary>
        /// 回溯路径（会考虑移动力，但不会考虑能否停留）
        /// </summary>
        public void Recall(List<Node> ret = null)
        {
            if (ret == null) 
                return;
            List<Node> stack = new();
            for (Node n = this; n != null; n = n.Parent)
                if (process.mover.MoveAbilityCheck(n))
                    stack.Add(n);
            for (int i = stack.Count - 1; i >= 0; i--)
                ret.Add(stack[i]);
        }

        public override string ToString()
        {
            return Position.ToString();
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

