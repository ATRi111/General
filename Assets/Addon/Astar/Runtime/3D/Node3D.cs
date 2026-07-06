using UnityEngine;

namespace AStar.ThreeD
{
    [System.Serializable]
    public class Node3D : Node
    {
        protected internal readonly PathFinding3DProcess process;
        [SerializeField]
        protected Vector3Int position;
        public Vector3Int Position => position;

        public override float WeightedFCost => process.HCostWeight * HCost + GCost;

        protected internal override MoverBase Mover => process.mover;

        /// <summary>
        /// 强类型的父节点访问
        /// </summary>
        public new Node3D Parent
        {
            get => (Node3D)base.Parent;
            set => base.Parent = value;
        }

        public Node3D(PathFinding3DProcess process, Vector3Int position)
        {
            this.process = process;
            this.position = position;
        }

        protected internal override float PrimitiveCostTo(Node to)
        {
            float distance = process.settings.CalculateDistance(Position, ((Node3D)to).Position);
            return distance;
        }

        public override string ToString()
        {
            return Position.ToString();
        }
    }
}
