using System.Collections.Generic;
using UnityEngine;

namespace AStar
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
        /// ��㵽�õ�ľ��루·����ȷ����
        /// </summary>
        public float GCost;

        /// <summary>
        /// �õ㵽�յ�ľ��루�������ϰ���
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
        /// ��ǰ�ڵ���ֱ�ӵ��ﵽĿ��ڵ�ʱ,������������
        /// </summary>
        public virtual float CostTo(Node to)
        {
            float primitiveCost = PrimitiveCostTo(to);
            return process.mover.CalculateCost(this, to, primitiveCost);
        }
        /// <summary>
        /// ��ǰ�ڵ㲻��ֱ�ӵ���Ŀ��ڵ�ʱ,Ԥ����������
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
        /// ����·�����ῼ���ƶ����������ῼ���ܷ�ͣ����
        /// </summary>
        public void Recall(List<Node> ret = null)
        {
            Parent?.Recall(ret);
            if (process.mover.MoveAbilityCheck(this))
                ret?.Add(this);
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
            return (int)Mathf.Sign(x.WeightedFCost - y.WeightedFCost);
        }
    }
}

