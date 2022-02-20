using System.IO;

public static class TextTool
{
    /// <summary>
    /// 读取（由空行分隔的)一段文本
    /// </summary>
    /// <param name="maxLine">最大读取行数</param>
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