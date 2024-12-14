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

        public virtual bool MoveAbilityCheck(Node node)
        {
            return Mathf.RoundToInt(node.GCost) <= Movability;
        }

        public virtual bool StayCheck(Node node)
        {
            return !node.IsObstacle;
        }

        public virtual bool MoveCheck(Node from, Node to)
        {
            return !to.IsObstacle;
        }

        public virtual float CalculateCost(Node from, Node to, float primitiveCost)
        {
            if (!StayCheck(to))
                primitiveCost += PathFindingUtility.Epsilon;
            return primitiveCost;
        }
    }
}