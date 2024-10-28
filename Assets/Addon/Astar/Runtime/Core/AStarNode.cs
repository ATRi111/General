using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    [System.Serializable]
    public class AStarNode
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

        private AStarNode parent;
        public AStarNode Parent
        {
            get => parent;
            set
            {
                parent = value;
                GCost = value == null ? 0 : Parent.GCost + value.CostTo(this);
            }
        }

        public AStarNode(PathFindingProcess process, Vector2Int position)
        {
            this.process = process;
            this.position = position;
            state = ENodeState.Blank;
        }


        public float CostTo(AStarNode to)
        {
            float primitiveCost = PrimitiveCostTo(to);
            return process.mover.CalculateCost(this, to, primitiveCost);
        }
        protected internal virtual float PrimitiveCostTo(AStarNode to)
        {
            float distance = process.Settings.CalculateDistance(Position, to.Position);
            return distance;
        }
        public void UpdateParent(AStarNode node)
        {
            float g = node.GCost + node.CostTo(this);
            if (GCost > g)
                Parent = node;
        }

        /// <summary>
        /// 回溯路径
        /// </summary>
        public void Recall(List<AStarNode> ret = null)
        {
            Parent?.Recall(ret);
            if (process.mover.MoveAbilityCheck(this) && process.mover.StayCheck(this))
                ret?.Add(this);
        }

        public override string ToString()
        {
            return Position.ToString();
        }
    }

    public class Comparer_Cost : IComparer<AStarNode>
    {
        public int Compare(AStarNode x, AStarNode y)
        {
            return (int)Mathf.Sign(x.WeightedFCost - y.WeightedFCost);
        }
    }
}

