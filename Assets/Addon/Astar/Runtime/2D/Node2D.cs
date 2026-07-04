using UnityEngine;
using AStar;

namespace AStar.TwoD
{
    [System.Serializable]
    public class Node2D : Node
    {
        protected internal readonly PathFindingProcess process;
        [SerializeField]
        protected Vector2Int position;
        public Vector2Int Position => position;

        /// <summary>
        /// 网格坐标在三维空间中的表示（z 恒为 0），仅用于跨空间表示的通用场景
        /// </summary>
        public override Vector3 Position3D => new(position.x, position.y, 0);

        public override float WeightedFCost => process.HCostWeight * HCost + GCost;

        protected internal override MoverBase Mover => process.mover;

        /// <summary>
        /// 强类型的父节点访问
        /// </summary>
        public new Node2D Parent
        {
            get => (Node2D)base.Parent;
            set => base.Parent = value;
        }

        public Node2D(PathFindingProcess process, Vector2Int position)
        {
            this.process = process;
            this.position = position;
        }

        protected internal override float PrimitiveCostTo(Node to)
        {
            float distance = process.settings.CalculateDistance(Position, ((Node2D)to).Position);
            return distance;
        }

        public override string ToString()
        {
            return Position.ToString();
        }
    }
}
