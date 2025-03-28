using System;
using UnityEngine;

namespace AStar
{
    public class MoverBase
    {
        public Func<float> GetMoveAbility;
        public float moveAbility = 5;
        public float Movability
        {
            get
            {
                if(GetMoveAbility != null) 
                    return GetMoveAbility();
                return moveAbility;
            }
        }

        /// <summary>
        /// 判断移动力是否足以到达某节点
        /// </summary>
        public virtual bool MoveAbilityCheck(Node node)
        {
            return Mathf.RoundToInt(node.GCost) <= Movability;
        }

        /// <summary>
        /// 判断最终能否停留在某个节点
        /// </summary>
        public virtual bool StayCheck(Node node)
        {
            return !node.IsObstacle;
        }

        /// <summary>
        /// 判断能否从某节点移动到另一节点
        /// </summary>
        public virtual bool MoveCheck(Node from, Node to)
        {
            return !to.IsObstacle;
        }

        /// <summary>
        /// 在原始距离的基础上，计算两点间距离
        /// </summary>
        public virtual float CalculateCost(Node from, Node to, float primitiveCost)
        {
            if (!StayCheck(to))
                primitiveCost += PathFindingUtility.Epsilon;    //优先选择经过可以停留的节点
            return primitiveCost;
        }
    }
}