using System;
using UnityEngine;

namespace AStar
{
    [Serializable]
    public class MoverBase
    {
        public const float Epsilon = 1e-6f;

        public float moveAbility = 5;
        public float MoveAbility => moveAbility;

        /// <summary>
        /// 判断移动力是否足以到达某节点
        /// </summary>
        public virtual bool MoveAbilityCheck(Node node)
        {
            return Mathf.RoundToInt(node.GCost) <= MoveAbility;
        }

        /// <summary>
        /// 判断最终能否停留在某个节点
        /// </summary>
        public virtual bool StayCheck(Node node)
        {
            return !node.IsObstacle;
        }

        /// <summary>
        /// 判断能否从某节点移动到另一节点，此函数被用于创建节点阶段，因此必须包含对to的null检查
        /// </summary>
        public virtual bool MoveCheck(Node from, Node to)
        {
            if(to == null) 
                return false;
            return !to.IsObstacle;
        }

        /// <summary>
        /// 在原始距离的基础上，计算两点间距离
        /// </summary>
        public virtual float CalculateCost(Node from, Node to, float primitiveCost)
        {
            if (!StayCheck(to))
                primitiveCost += Epsilon;    //优先选择经过可以停留的节点
            return primitiveCost;
        }
    }
}
