using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

namespace Tools
{
    public static partial class TextTool
    {
        /// <summary>
        /// ��ȡ���ɿ��зָ���)һ���ı�
        /// </summary>
        /// <param name="maxLine">����ȡ����</param>
        public static string GetParagraph(StreamReader reader, int maxLine)
        {
            StringBuilder ret = null;
            string line;
            try
            {
                for (int i = 0; i < maxLine; i++)
                {
                    line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line))
                        break;
                    ret.Append(line).Append('\n');
                }
            }
            catch (Exception e)
            {
                Debug.Log(e);
            }
            return ret.ToString();
        }

        /// <summary>
        /// ���һ���ַ����Ļ��У������»���
        /// </summary>
        /// <param name="characterPerLine">ÿ��������0��ʾ������</param>
        public static string Wrap(string s, int characterPerLine)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != '\n')
                    sb.Append(s[i]);
            }
            if (characterPerLine <= 0)
                return sb.ToString();
            int count = 0;
            for (int i = 0; i < sb.Length; i++, count++)
            {
                if (count == characterPerLine)
                {
                    sb.Insert(i, '\n');
                    count = 0;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// �ϲ������ַ����������ַ���֮�����û�л��У����任��(���ַ����ᱻ����)
        /// </summary>
        public static string CombineAndWrap(params string[] s)
        {
            StringBuilder sb = new StringBuilder();
            List<string> processed = new List<string>();
            foreach (string str in s)
            {
                if (!string.IsNullOrEmpty(str))
                    processed.Add(str);
            }
            string prev, post = null;
            for (int i = 0; i < processed.Count - 1; i++)
            {
                prev = processed[i];
                post = processed[i + 1];
                sb.Append(prev);
                if (prev[prev.Length - 1] != '\n' && post[0] != '\n')
                    sb.Append('\n');
            }
            sb.Append(post);
            return sb.ToString();
        }
    }
}

