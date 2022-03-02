using System.IO;
using UnityEngine;

public static class TextureTool
{
    public static readonly Vector2 s_mid = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// ��ȡͼƬ��ת��ΪTexture2D
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Texture2D LoadImage(string path)
    {
        FileInfo fileInfo = FileTool.GetFileInfo(path);
        using FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, (int)fileStream.Length);

        Texture2D ret = new Texture2D(0, 0);
        ret.LoadImage(bytes);
        return ret;
    }

    /// <summary>
    /// ����һ��ͼƬ����ԭ��������ʾ
    /// </summary>
    public static Sprite LoadSptite(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), s_mid);
        return sprite;
    }

    /// <summary>
    /// ��Texture2D���ΪͼƬ
    /// </summary>
    /// <param name="filePath">�ļ�·������Ӧ�����ļ���ʽ</param>
    public static void CreateImage(string filePath, Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        FileStream file = File.Open(filePath + ".png", FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
    }
}