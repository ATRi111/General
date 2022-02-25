using System.IO;
using UnityEngine;
using static SpriteScaleMode;

public enum SpriteScaleMode
{
    /// <summary>
    /// �����������
    /// </summary>
    Free,
    /// <summary>
    /// ���ı���
    /// </summary>
    WidthOnly,
    /// <summary>
    /// ���ı�߶�
    /// </summary>
    HeightOnly,
    /// <summary>
    /// �ı��ȣ��߶ȵȱ�������
    /// </summary>
    ByWidth,
    /// <summary>
    /// �ı�߶ȣ���ȵȱ�������
    /// </summary>
    ByHeight,
}

public static class SpriteTool
{
    private const float HEIGHT_CAMERA = 10f;    //����߶� = ����ߴ� * 2
    private const float WIDTH_CAMERA = 17.78f;  //������ = ������ * �����߱�

    public static Vector2 s_mid = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// ����һ��ͼƬ����ԭ��������ʾ��Pixel Per UnitΪ100��
    /// </summary>
    /// <param name="path">Դ�ļ�·��</param>
    public static Sprite LoadSptite(string path)
    {
        FileInfo fileInfo = FileTool.GetFileInfo(path);
        using FileStream fileStream = new FileStream(fileInfo.FullName, FileMode.Open, FileAccess.Read);
        byte[] bytes = new byte[fileStream.Length];
        fileStream.Read(bytes, 0, (int)fileStream.Length);

        Texture2D texture = new Texture2D(0, 0);
        texture.LoadImage(bytes);
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), s_mid);

        return sprite;
    }

    /// <summary>
    /// ����Sprite�Ŀ��
    /// </summary>
    /// <param name="width">Ŀ���ȣ�����Ļ��ȵı����ƣ�</param>
    /// <param name="height">Ŀ��߶ȣ�����Ļ�߶ȵı����ƣ�</param>
    /// <param name="scaleMode">�α�ģʽ</param>
    /// <returns>����Ϸ�����lossyScaleӦ����Ϊ����</returns>
    public static Vector3 ScaleWithScreen(SpriteRenderer spriteRenderer, float width, float height, SpriteScaleMode scaleMode = Free)
    {
        if (spriteRenderer == null)
            return Vector3.one;
        GameObject obj = spriteRenderer.gameObject;
        if (obj == null)
            return Vector3.one;
        Vector3 original = obj.transform.localScale;
        Sprite sprite = spriteRenderer.sprite;
        if (sprite == null)
            return original;

        float pixelsPerUnit = sprite.pixelsPerUnit;
        float kx, ky;
        switch (scaleMode)
        {
            case WidthOnly:
                kx = WIDTH_CAMERA * pixelsPerUnit/ sprite.rect.width * width;
                ky = original.y;
                break;
            case HeightOnly:
                kx = original.x;
                ky = HEIGHT_CAMERA * pixelsPerUnit / sprite.rect.height * height;
                break;
            case Free:
                kx = WIDTH_CAMERA * pixelsPerUnit / sprite.rect.width * width;
                ky = HEIGHT_CAMERA * pixelsPerUnit / sprite.rect.height * height;
                break;
            case ByWidth:
                kx = WIDTH_CAMERA * pixelsPerUnit / sprite.rect.width * width;
                ky = kx;
                break;
            case ByHeight:
                ky = HEIGHT_CAMERA * pixelsPerUnit / sprite.rect.height * height;
                kx = ky;
                break;
            default:
                kx = original.x;
                ky = original.y;
                break;
        }
        return new Vector3(kx, ky, 1f);
    }
}
