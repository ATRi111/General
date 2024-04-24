using UnityEngine;

namespace MyTimer
{
    public class StringLerp : ILerp<string>
    {
        public string Value(string origin, string target, float percent, float time, float duration)
        {
            return target[..Mathf.CeilToInt(percent * target.Length)];
        }
    }
}