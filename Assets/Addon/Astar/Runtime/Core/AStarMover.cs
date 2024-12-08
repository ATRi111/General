using System;
using UnityEngine;

namespace AStar
{
    public class AStarMover
    {
        public Func<float> MoveAbility;

        public virtual bool MoveAbilityCheck(AStarNode node)
        {
            return Mathf.RoundToInt(node.GCost) <= MoveAbility();
        }

        public virtual bool StayCheck(AStarNode node)
        {
            return !node.IsObstacle;
        }

        public virtual bool MoveCheck(AStarNode from, AStarNode to)
        {
            return !to.IsObstacle;
        }

        public virtual float CalculateCost(AStarNode from, AStarNode to, float primitiveCost)
        {
            if (!StayCheck(to))
                primitiveCost += PathFindingUtility.Epsilon;
            return primitiveCost;
        }
    }
}