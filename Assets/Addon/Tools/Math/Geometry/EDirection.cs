using System;
using System.Collections.Generic;
using UnityEngine;

namespace Tools
{
    public enum EDirection
    {
        Up,
        LeftUp,
        Left,
        LeftDown,
        Down,
        RightDown,
        Right,
        RightUp,
        None,
    }

    public static class EDirectionTool
    {

        private static readonly Vector2Int[] Vectors = new Vector2Int[]
        {
        Vector2Int.up,
        Vector2Int.left + Vector2Int.up,
        Vector2Int.left,
        Vector2Int.left + Vector2Int.down,
        Vector2Int.down,
        Vector2Int.right + Vector2Int.down,
        Vector2Int.right,
        Vector2Int.right + Vector2Int.up,
        Vector2Int.zero,
        };

        private static readonly Dictionary<EDirection, Vector2Int> Dict_Direction = new Dictionary<EDirection, Vector2Int>();
        private static readonly Dictionary<Vector2Int, EDirection> Dict_EDirection = new Dictionary<Vector2Int, EDirection>();

        static EDirectionTool()
        {
            for (int i = 0; i < Vectors.Length; i++)
            {
                Dict_Direction.Add((EDirection)i, Vectors[i]);
                Dict_EDirection.Add(Vectors[i], (EDirection)i);
            }
        }

        public static EDirection ToEDirection(this Vector2Int direction)
        {
            if (Dict_EDirection.ContainsKey(direction))
                return Dict_EDirection[direction];
            Debug.LogWarning($"����ķ���Ϊ{direction}�����ǰ��ַ���֮һ");
            return EDirection.None;
        }

        public static Vector2Int ToVector(this EDirection eDirection)
            => Dict_Direction[eDirection];

        public static float ToAngle(this EDirection eDirection)
            => (int)eDirection * 45f;

        public static bool IsOblique(this EDirection eDirection)
        {
            switch (eDirection)
            {
                case EDirection.Up:
                case EDirection.Left:
                case EDirection.Down:
                case EDirection.Right:
                case EDirection.None:
                    return false;
                default:
                    return true;
            }
        }

        /// <summary>
        /// ����ʱ��˳�򷵻ذ˸�����
        /// </summary>
        public static List<Vector2Int> GetDirections()
        {
            List<Vector2Int> ret = new List<Vector2Int>(8);
            for (int i = 0; i < 8; i++)
            {
                ret.Add(((EDirection)i).ToVector());
            }
            return ret;
        }

        /// <summary>
        /// ��˳�򷵻ذ˸�������direction���ӽ�����ǰ
        /// </summary>
        /// <param name="n">�������ǰn��ֵ</param>
        public static List<Vector2Int> NearerDirections(Vector2 direction, int n = 8)
        {
            n = Mathf.Clamp(n, 0, 8);
            List<Vector2Int> ret = GetDirections();
            GeometryTool.Comparer_Vector2_Nearer comparer = new GeometryTool.Comparer_Vector2_Nearer(direction);
            ret.Sort(comparer);
            return ret.GetRange(0, n);
        }

        /// <summary>
        /// ���ذ˸�������������������ӽ���
        /// </summary>
        public static Vector2Int NearestDirection(Vector2 direciton)
        {
            int sign = Mathf.RoundToInt(direciton.ToAngle() / 45f) % 8;
            return Vectors[sign];
        }
        /// <summary>
        /// �����ĸ�������������������ӽ���
        /// </summary>
        public static Vector2Int NearestDirection4(Vector2 direciton)
        {
            int sign = Mathf.RoundToInt(direciton.ToAngle() / 90f) % 4 * 2;
            return Vectors[sign];
        }

        /// <summary>
        /// ��λ�Ʒֽ�ɰ����ƶ�����ϣ�����ʹ����ӽ�ֱ��
        /// </summary>
        /// <returns>·���ϵ�ÿһ�㣨������㣩</returns>
        public static List<Vector2Int> DivideDisplacement(Vector2Int origin, Vector2Int target, int maxCount = 20)
        {
            Vector2Int offset = target - origin;
            int sign = (int)(((Vector2)offset).ToAngle() / 45f);
            Vector2Int base1, base2;
            int c1, c2;
            switch (sign)
            {
                case 0:
                    base1 = Vector2Int.up;
                    base2 = Vector2Int.up + Vector2Int.left;
                    break;
                case 1:
                    base1 = Vector2Int.left;
                    base2 = Vector2Int.up + Vector2Int.left;
                    break;
                case 2:
                    base1 = Vector2Int.left;
                    base2 = Vector2Int.down + Vector2Int.left;
                    break;
                case 3:
                    base1 = Vector2Int.down;
                    base2 = Vector2Int.down + Vector2Int.left;
                    break;
                case 4:
                    base1 = Vector2Int.down;
                    base2 = Vector2Int.down + Vector2Int.right;
                    break;
                case 5:
                    base1 = Vector2Int.right;
                    base2 = Vector2Int.down + Vector2Int.right;
                    break;
                case 6:
                    base1 = Vector2Int.right;
                    base2 = Vector2Int.up + Vector2Int.right;
                    break;
                case 7:
                    base1 = Vector2Int.up;
                    base2 = Vector2Int.up + Vector2Int.right;
                    break;
                default:
                    base1 = Vector2Int.up;
                    base2 = Vector2Int.one;
                    break;
            }
            int x = Mathf.Abs(offset.x);
            int y = Mathf.Abs(offset.y);
            c1 = Mathf.Abs(x - y);
            c2 = Mathf.Min(x, y);
            int count = c1 + c2;
            if (count > maxCount)
            {
                c1 = Mathf.RoundToInt((float)c1 / count * maxCount);
                c2 = Mathf.RoundToInt((float)c2 / count * maxCount);
            }
            count = c1 + c2;
            List<int> mix = MathTool.MixList(c1, c2);
            Vector2Int[] bases = new Vector2Int[] { base1, base2 };
            Vector2Int current = origin;
            List<Vector2Int> ret = new List<Vector2Int> { origin };
            for (int i = 0; i < count; i++)
            {
                current += bases[mix[i]];
                ret.Add(current);
            }
            return ret;
        }

        /// <summary>
        /// ��һ��ֱ��ǰ�����ڲ���������ʱͣ�£�ֱ����ÿһ��������ʾ��
        /// </summary>
        /// <param name="CanMove">���������������ֱ��ʾһ���������յ�</param>
        /// <param name="containStop">���ؽ���Ƿ����ʹֱ��ͣ�µĵ�</param>
        /// <param name="containStart">���ؽ���Ƿ�������</param>
        /// <returns>ֱ��ֱ��ͣ��֮ǰ�Ĳ���</returns>
        public static List<Vector2Int> StopLine(List<Vector2Int> line, Func<Vector2Int, Vector2Int, bool> CanMove, bool containStop = false, bool containStart = false)
        {
            List<Vector2Int> ret = new List<Vector2Int> { line[0] };
            for (int i = 0; i < line.Count - 1; i++)
            {
                if (!CanMove(line[i], line[i + 1]))
                {
                    if (containStop)
                        ret.Add(line[i + 1]);
                    break;
                }
                ret.Add(line[i + 1]);
            }
            if (!containStart)
                ret.RemoveAt(0);
            return ret;
        }
    }
}
