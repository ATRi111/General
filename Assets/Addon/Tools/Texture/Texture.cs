using System.IO;
using UnityEngine;

public static class TextureTool
{
    public static readonly Vector2 s_mid = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// ��ȡͼƬ��ת��ΪTexture2D
    /// </summary>
    public static Texture2D LoadImage(string path)
    {
        FileInfo fileInfo = new FileInfo(path);
        if (!fileInfo.Exists)
            return null;
        using FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, (int)fileStream.Length);

        Texture2D ret = new Texture2D(0, 0);
        ret.LoadImage(bytes);
        return ret;
    }

    /// <summary>
    /// ����һ��ͼƬ����ԭ��������ʾ���˷���ֻ����������ʱ��
    /// </summary>
    public static Sprite LoadSptite(Texture2D texture)
    {
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), s_mid);
        sprite.name = texture.name;
        return sprite;
    }

    /// <summary>
    /// ��RenderTexture����Texture2D�����ɺ���Ҫ�ֶ�����
    /// </summary>
    public static Texture2D CreateTexture2D(RenderTexture renderTexture)
    {
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
        RenderTexture.active = renderTexture;
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();
        return texture2D;
    }

    /// <summary>
    /// ��Texture2D���ΪͼƬ
    /// </summary>
    /// <param name="filePath">�ļ�·����Ҫ�����ļ���ʽ</param>
    public static void CreateImage(string filePath, Texture2D texture)
    {
        byte[] bytes = texture.EncodeToPNG();
        FileStream file = File.Open(filePath, FileMode.Create);
        BinaryWriter writer = new BinaryWriter(file);
        writer.Write(bytes);
    }
}