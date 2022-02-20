using System.IO;

public static class TextTool
{
    /// <summary>
    /// ��ȡ���ɿ��зָ���)һ���ı�
    /// </summary>
    /// <param name="maxLine">����ȡ����</param>
    public static string GetParagraph(StreamReader reader, int maxLine)
    {
        string ret = null;
        string line;
        try
        {
            for (int i = 0; i < maxLine; i++)
            {
                line = reader.ReadLine();
                if (string.IsNullOrWhiteSpace(line))
                    break;
                ret += line + '\n';
            }
        }
        catch
        {
            ret = null;
        }
        return ret;
    }
}