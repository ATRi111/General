using UnityEngine;

public static class Useless
{
    public static Vector2 ToVector2(this string s)
    {
        if (s == "")
            return Vector2.zero;
        string[] strs = s.Split(',');
        try
        {
            float x = float.Parse(strs[0]);
            float y = float.Parse(strs[1]);
            return new Vector2(x, y);
        }
        catch
        {
            Debug.LogWarning($"����Ϊ��{s}������ʽ����ȷ");
            return Vector2.zero;
        }
    }

    /// <summary>
    /// ���������پ���
    /// </summary>
    /// <param name="includingY">�Ƿ����y����ľ���</param>
    /// <returns></returns>
    public static float ManhattanDistance(Vector3 a, Vector3 b, bool includingY = true)
        => Mathf.Abs(a.x - b.x) + (includingY ? Mathf.Abs(a.y - b.y) : 0f) + Mathf.Abs(a.z - b.z);

    //�ж�point�Ƿ��ڴ�start��end���߶ε���ࣨͶӰ��XYƽ�棩
    public static bool OnLeft(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 line1 = (end - start).ResetZ();
        Vector3 line2 = (point - start).ResetZ();
        return Vector3.Cross(line2, line1).z >= 0;
    }
    //�ж������ͬʱ������line1�Ƿ���line2����ࣨͶӰ��XYƽ�棩
    public static bool OnLeft(Vector3 line1, Vector3 line2)
    {
        line1 = line1.ResetZ();
        line2 = line2.ResetZ();
        return Vector3.Cross(line2, line1).z >= 0;
    }
    public static bool OnRight(Vector3 start, Vector3 end, Vector3 point)
    {
        Vector3 line1 = (end - start).ResetZ();
        Vector3 line2 = (point - start).ResetZ();
        return Vector3.Cross(line2, line1).z <= 0;
    }
    public static bool OnRight(Vector3 line1, Vector3 line2)
    {
        line1 = line1.ResetZ();
        line2 = line2.ResetZ();
        return Vector3.Cross(line2, line1).z <= 0;
    }
    /// <summary>
    /// ����˫���Բ�ֵ
    /// </summary>
    /// <returns>�ĸ������ֱ��ʾ���¡����ϡ����ϡ����µ�Ȩ��</returns>
    public static Vector4 Bilinear(float left, float right, float bottom, float up, float X, float Y)
    {
        if (left > right || bottom > up)
            throw new System.ArgumentException();
        float x, y;
        float alpha, beta, gamma, delta;
        x = (left == right) ? 0.5f : (X - left) / (right - left);
        y = (bottom == up) ? 0.5f : (Y - bottom) / (up - bottom);
        alpha = x * y;
        beta = x * (1f - y);
        delta = (1f - x) * y;
        gamma = 1f - alpha - beta - delta;
        return new Vector4(alpha, beta, gamma, delta);
    }
}
