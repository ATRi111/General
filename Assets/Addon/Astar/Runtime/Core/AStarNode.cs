using System.Collections.Generic;
using UnityEngine;

namespace AStar
{
    public class PathNode
    {
        private readonly PathFindingProcess process;

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

        /// <summary>
        /// ��㵽�õ�ľ��루·����ȷ����
        /// </summary>
        public float GCost;

        /// <summary>
        /// �õ㵽�յ�ľ��루�������ϰ���
        /// </summary>
        public float HCost;

        /// <summary>
        /// �����õ�ʱ����㵽�յ�ľ��루����õ㵽�յ����ϰ���
        /// </summary>
        public float FCost => process.CurrentWeight * HCost + GCost;

        private PathNode _Parent;
        //��һ������
        public PathNode Parent
        {
            get => _Parent;
            set
            {
                _Parent = value;
                GCost = value == null ? 0 : Parent.GCost + CalculateGCost(value);
            }
        }

        internal PathNode(PathFindingProcess process, Vector2Int position)
        {
            this.process = process;
            Position = position;
            Type = ENodeType.Blank;
        }

        public void UpdateHCost(PathNode to)
        {
            HCost = CalculateHCost(to);
        }

        public float CalculateHCost(PathNode other)
            => process.Settings.CalculateHCost(Position, other.Position);

        public float CalculateGCost(PathNode other)
            => process.Settings.CalculateGCost(Position, other.Position);

        /// <summary>
        /// ����·��
        /// </summary>
        public void Recall(List<PathNode> ret = null)
        {
            Type = ENodeType.Route;
            if (Parent != null)
                Parent.Recall(ret);
            if (ret != null)
                ret.Add(this);
        }

        public override string ToString()
        {
            return Position.ToString();
        }
    }

    public class Comparer_Cost : IComparer<PathNode>
    {
        public int Compare(PathNode x, PathNode y)
        {
            return (int)Mathf.Sign(x.FCost - y.FCost);
        }
    }
}

