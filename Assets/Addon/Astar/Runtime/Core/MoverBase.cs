using System;
using UnityEngine;

namespace AStar
{
    public class MoverBase
    {
        /// <summary>
        /// ���ڼ����ƶ����ķ������Ǳ���
        /// </summary>
        public Func<float> GetMoveAbility;
        public float moveAbility = 5;
        public float Movability
        {
            get
            {
                if (GetMoveAbility != null)
                    return GetMoveAbility();
                return moveAbility;
            }
        }

        /// <summary>
        /// �ж��ƶ����Ƿ����Ե���ĳ�ڵ�
        /// </summary>
        public virtual bool MoveAbilityCheck(Node node)
        {
            return Mathf.RoundToInt(node.GCost) <= Movability;
        }

        /// <summary>
        /// �ж������ܷ�ͣ����ĳ���ڵ�
        /// </summary>
        public virtual bool StayCheck(Node node)
        {
            return !node.IsObstacle;
        }

        /// <summary>
        /// �ж��ܷ��ĳ�ڵ��ƶ�����һ�ڵ�
        /// </summary>
        public virtual bool MoveCheck(Node from, Node to)
        {
            return !to.IsObstacle;
        }

        /// <summary>
        /// ��ԭʼ����Ļ����ϣ�������������
        /// </summary>
        public virtual float CalculateCost(Node from, Node to, float primitiveCost)
        {
            if (!StayCheck(to))
                primitiveCost += PathFindingUtility.Epsilon;    //����ѡ�񾭹�����ͣ���Ľڵ�
            return primitiveCost;
        }
    }
}