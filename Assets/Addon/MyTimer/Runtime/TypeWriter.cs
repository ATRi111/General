using UnityEngine;

namespace MyTimer
{
    /// <summary>
    /// 打字机效果。
    /// 在初始化时确定每个字符占用的时间，句子结束标点后的停顿作为额外时间
    /// 直接烘焙进时间线，因此无需额外的计时器，全程只用一个 Timer。
    /// </summary>
    public class TypeWriter : Timer<string, StringLerp>
    {
        /// <summary>
        /// 所有标志句子结束的标点（默认）
        /// </summary>
        public const string SentenceSeparator = "?!.。！？…";

        /// <param name="text">文本</param>
        /// <param name="letterPerSecond">每秒打的字符数</param>
        /// <param name="interval">每个句子结束后的停留时间</param>
        /// <param name="separator">所有标志句子结束的标志，为空则使用默认的结束标志</param>
        /// <param name="start">是否启动计时器</param>
        public void Initialize(string text, float letterPerSecond, float interval = 0f, string separator = null, bool start = true)
        {
            separator ??= SentenceSeparator;
            int len = text.Length;
            float step = 1f / Mathf.Max(letterPerSecond, 1e-5f);

            // 前缀和时间表：prefixSum[i] 表示第 i 个字符“开始显示”的时刻
            float[] prefixSum = new float[len + 1];
            float t = 0f;
            for (int i = 0; i < len; i++)
            {
                float d = step;
                bool isSep = separator.IndexOf(text[i]) != -1;
                bool nextIsSep = i + 1 < len && separator.IndexOf(text[i + 1]) != -1;
                // 仅当下一个字符不是分隔符时才附加停顿，避免连续标点（如“？！”）被重复延时
                if (isSep && !nextIsSep)
                {
                    d += interval;
                }
                t += d;
                prefixSum[i + 1] = t;
            }

            base.Initialize("", text, t, start);
            (Lerp as StringLerp).Initialize(prefixSum);
        }
    }
}
