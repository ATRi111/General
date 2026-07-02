namespace MyTimer
{
    /// <summary>
    /// 字符串插值：按“前缀和时间表”把已流逝的时间映射为已显示的字符数。
    /// prefixSum[i] 表示第 i 个字符开始显示的时刻。
    /// </summary>
    public class StringLerp : ILerp<string>
    {
        private float[] prefixSum;

        public void Initialize(float[] prefixSum) => this.prefixSum = prefixSum;

        public string Value(string origin, string target, float percent, float time, float duration)
        {
            int count = 0;
            int len = target.Length;
            while (count < len && count < prefixSum.Length && time >= prefixSum[count])
            {
                count++;
            }
            return target[..count];
        }
    }
}
