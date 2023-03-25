using System.Collections.Generic;
using System.Text;

namespace Tools
{
    public static partial class TextTool
    {
        /// <summary>
        /// �������ַ������δ���ԭ�ַ����е�ĳ���ַ�
        /// </summary>
        public static string OneToManyReplace(string origin, char c, params string[] variables)
        {
            StringBuilder sb = new StringBuilder();
            string[] split = origin.Split(c);

            for (int i = 0; i < split.Length - 1; i++)
            {
                sb.Append(split[i]);
                if (i < variables.Length)
                    sb.Append(variables[i]);
                else
                    sb.Append(c);
            }
            sb.Append(split[split.Length - 1]);
            return sb.ToString();
        }

        /// <summary>
        /// �Ƿ����һϵ���ַ����ַ���˫�䣬ʹ�����ַ�����ÿ���ַ�����ӳ���õ��Է�
        /// </summary>
        public static bool HasBijection(string a, string b)
        {
            if (a.Length != b.Length)
                return false;
            Dictionary<char, char> AToB = new Dictionary<char, char>();
            Dictionary<char, char> BToA = new Dictionary<char, char>();
            for (int i = 0; i < a.Length; i++)
            {
                if (!AToB.ContainsKey(a[i]))
                    AToB.Add(a[i], b[i]);
                else if (AToB[a[i]] != b[i])
                    return false;
                if (!BToA.ContainsKey(b[i]))
                    BToA.Add(b[i], a[i]);
                else if (BToA[b[i]] != a[i])
                    return false;
            }
            return true;
        }
    }
}