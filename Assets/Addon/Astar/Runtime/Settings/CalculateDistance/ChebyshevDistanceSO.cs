using UnityEngine;

namespace AStar
{
    [CreateAssetMenu(fileName = "�б�ѩ�����", menuName = "AStar/������������ķ���/�б�ѩ�����")]
    public class ChebyshevDistanceSO : CalculateDistanceSO
    {
        public override float CalculateDistance(Vector2Int from, Vector2Int to)
        {
            return PathFindingUtility.ChebyshevDistance(from, to);
        }
    }
}