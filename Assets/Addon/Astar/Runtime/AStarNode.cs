using System.Collections.Generic;
using UnityEngine;
using Tools;

namespace AStarOnGrid
{
    public class AStarNode
    {
        public const int Side = 10;     //边长
        public const int Diagnol = 14;  //对角线长
        public Vector2Int Position { get; private set; }

        private ENodeType type;
        public ENodeType Type
        {
            get => type;
            set
            {
                type = value;
            }
        }

        public int GCost;

        /// <summary>
        /// 该点到终点的距离（假设无障碍）
        /// </summary>
        public int HCost;

        /// <summary>
        /// 经过该点时，起点到终点的距离（假设该点到终点无障碍）
        /// </summary>
        public float FCost => PathFinding.Weight * HCost + GCost;

        private AStarNode _Parent;
        //上一个方块
        public AStarNode Parent
        {
            get => _Parent;
            set
            {
                _Parent = value;
                GCost = value == null ? 0 : Parent.GCost + CalculateDistance(value);
            }
        }

        internal AStarNode(Vector2Int position)
        {
            Position = position;
            Type = ENodeType.Blank;
        }

        public void CalculateHCost(AStarNode end)
        {
            HCost = CalculateDistance(end);
        }

        public int CalculateDistance(AStarNode other)
        {
            return GeometryTool.ChebyshevDistanceInt(Position, other.Position, Side, Diagnol);
        }

        /// <summary>
        /// 回溯路径
        /// </summary>
        public void Recall(List<Vector2Int> ret = null)
        {
            Type = ENodeType.Route;
            if (Parent != null)
                Parent.Recall(ret);
            if (ret != null)
                ret.Add(Position);
        }
    }

    public class Comparer_Cost : IComparer<AStarNode>
    {
        public int Compare(AStarNode x, AStarNode y)
        {
            return (x.FCost - y.FCost).Sign();
        }
    }
}

