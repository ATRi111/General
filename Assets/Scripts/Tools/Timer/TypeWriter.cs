using UnityEngine;

/// <summary>
/// ���ֻ�Ч��
/// </summary>
public class TypeWriter : MyTimer<string>
{
    /// <param name="text">�ı�</param>
    /// <param name="letterPerSecond">ÿ�����ַ���</param>
    /// <param name="start">�Ƿ�������ʱ��</param>
    public void Initialize(string text, float letterPerSecond, bool start = true)
    {
        base.Initialize("", text, text.Length / letterPerSecond, start);
    }

    /// <summary>
    /// ��ǰ������ı�
    /// </summary>
    public override string Current => Target.Substring(0, Mathf.FloorToInt(Percent * Target.Length));
}
