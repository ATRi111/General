using System.Collections.Generic;
using UnityEngine;

namespace MyTool
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

        private static readonly Dictionary<EDirection, Vector2Int> Dict_Direction = new();
        private static readonly Dictionary<Vector2Int, EDirection> Dict_EDirection = new();

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
            return eDirection switch
            {
                EDirection.Up or EDirection.Left or EDirection.Down or EDirection.Right or EDirection.None => false,
                _ => true,
            };
        }

        /// <summary>
        /// ����ʱ��˳�򷵻ذ˸�����
        /// </summary>
        public static List<Vector2Int> GetDirections()
        {
            List<Vector2Int> ret = new(8);
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
            GeometryTool.Comparer_Vector2_Nearer comparer = new(direction);
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
        /// �����߶θ��ǵ�����Ϊ�����ķ����߶δ�from�����ĵ����굽to�����ĵ㣩
        /// </summary>
        public static void OverlapInt(Vector2Int from, Vector2Int to, List<Vector2Int> ret)
        {
            ret.Clear();
            ret.Add(from);
            OverlapInt(to - from, ret);
        }

        private static void OverlapInt(Vector2Int v, List<Vector2Int> ret, int gcd = -1)
        {
            void Add(Vector2Int v)
            {
                ret.Add(ret[^1] + v);
            }

            if (v.x == 0 && v.y == 0)
                return;

            int deltaX = v.x;
            int deltaY = v.y;
            int absx = Mathf.Abs(deltaX);
            int absy = Mathf.Abs(deltaY);
            Vector2Int x, y;

            if (v.x == 0)
            {
                y = new(0, deltaY / absy);
                for (int i = 0; i < absy; i++)
                    Add(y);
                return;
            }

            if (v.y == 0)
            {
                x = new(deltaX / absx, 0);
                for (int i = 0; i < absx; i++)
                    Add(x);
                return;
            }

            if (gcd == -1)
                gcd = MathTool.GreatestCommonDivisor(absx, absy);

            if (gcd > 1)
            {
                for (int i = 0; i < gcd; i++)
                    OverlapInt(new Vector2Int(deltaX / gcd, deltaY / gcd), ret, 1);
                return;
            }

            //Լ�ֺ�absx/absy��Ϊһ��һż����Ȼ���Էֽ�Ϊ����x/y����ϣ�����Ϊ��������Ȼ���Էֽ�Ϊ��һ��x+y������x/y�����
            x = new(deltaX / absx, 0);
            y = new(0, deltaY / absy);
            Vector2Int[] bases = new Vector2Int[] { x, y };
            if ((absx & 1) == 0 || (absy & 1) == 0)
            {
                List<int> mix = MathTool.MixList(absx, absy);
                for (int i = 0; i < mix.Count; i++)
                    Add(bases[mix[i]]);
            }
            else
            {
                List<int> mix = MathTool.MixList(absx / 2, absy / 2);
                for (int i = 0; i < mix.Count; i++)
                    Add(bases[mix[i]]);
                Add(x + y);
                for (int i = 0; i < mix.Count; i++)
                    Add(bases[mix[i]]);
            }
        }
    }
}
