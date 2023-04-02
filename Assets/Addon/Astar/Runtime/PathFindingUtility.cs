using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace AStar
{
    public static class PathFindingUtility
    {
        //�߳�
        public static float Side = 10f;
        //�Խ��߳�
        public static float Diagnol = 14f;

        /// <summary>
        /// Ĭ�ϵ����ڼ���Hcost��Weight�ķ���
        /// </summary>
        public static float CalculateWeight_Default(PathFindingProcess _)
        {
            return 1f;
        }
        /// <summary>
        /// Ĭ�ϵ����ڼ������ڵ���ܷ��ƶ��ķ������������Ƿ����ڣ�
        /// </summary>
        public static bool CheckPassable_Default(PathNode _, PathNode to)
        {
            return to.Type != ENodeType.Obstacle;
        }
        /// <summary>
        /// Ĭ�ϵ�����ȷ���ڵ����͵ķ���
        /// </summary>
        public static ENodeType DefineNodeType_Default(Vector2Int _)
        {
            return ENodeType.Blank;
        }

        #region ����Ѱ·
        /// <summary>
        /// �ĸ�����������ļ���
        /// </summary>
        public static readonly ReadOnlyCollection<Vector2Int> fourDirections;
        /// <summary>
        /// �������پ���
        /// </summary>
        public static float ManhattanDistance(Vector2Int a, Vector2Int b)
            => Mathf.Abs(a.x - b.x) * Side + Mathf.Abs(a.y - b.y) * Side;

        /// <summary>
        /// ��ȡĳ�ڵ���Χ���ĸ��ڵ�
        /// </summary>
        public static void GetAdjoinNodes_Four(PathFindingProcess process, PathNode node, List<PathNode> ret)
        {
            ret.Clear();
            foreach (Vector2Int direction in fourDirections)
            {
                ret.Add(process.GetNode(node.Position + direction));
            }
        }

        #endregion

        #region ����Ѱ·

        /// <summary>
        /// �˸�����������ļ���
        /// </summary>
        public static readonly ReadOnlyCollection<Vector2Int> eightDirections;
        /// <summary>
        /// ���б�ѩ�����
        /// </summary>
        public static float ChebyshevDistance(Vector2Int a, Vector2Int b)
        {
            float deltaX = Mathf.Abs(a.x - b.x);
            float deltaY = Mathf.Abs(a.y - b.y);
            float max = Mathf.Max(deltaX, deltaY);
            float min = Mathf.Min(deltaX, deltaY);
            return min * Diagnol + (max - min) * Side;
        }
        /// <summary>
        /// ��ȡĳ�ڵ���Χ�İ˸��ڵ�
        /// </summary>
        public static void GetAdjoinNodes_Eight(PathFindingProcess process, PathNode node, List<PathNode> ret)
        {
            ret.Clear();
            foreach (Vector2Int direction in eightDirections)
            {
                ret.Add(process.GetNode(node.Position + direction));
            }
        }
        #endregion

        static PathFindingUtility()
        {
            Vector2Int[] eight = new Vector2Int[]
            {
                Vector2Int.up,
                Vector2Int.left + Vector2Int.up,
                Vector2Int.left,
                Vector2Int.left + Vector2Int.down,
                Vector2Int.down,
                Vector2Int.right + Vector2Int.down,
                Vector2Int.right,
                Vector2Int.right + Vector2Int.up,
            };
            eightDirections = new ReadOnlyCollection<Vector2Int>(eight);
            Vector2Int[] four = new Vector2Int[]
            {
                 Vector2Int.up,
                Vector2Int.left,
                Vector2Int.down,
                Vector2Int.right,
            };
            fourDirections = new ReadOnlyCollection<Vector2Int>(four);
        }
    }
}