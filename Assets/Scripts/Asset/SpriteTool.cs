using System.IO;
using UnityEngine;
using static SpriteFillMode;

public enum SpriteFillMode
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
    ByWidthEqually,
    /// <summary>
    /// �ı�߶ȣ���ȵȱ�������
    /// </summary>
    ByHeightEqually,
}

public static class SpriteTool
{
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

    private const float CAMERA_HEIGHT = 10f;    //�����ȣ���unit�ƣ��޸������С������Ҳ��Ҫ�ֶ��޸ģ�
    private const float CAMERA_WIDTH = 17.78f;  //����߶ȣ���unit�ƣ��޸������С������Ҳ��Ҫ�ֶ��޸ģ�


    /// <summary>
    /// ����ͼƬ�Ŀ��
    /// </summary>
    /// <param name="width">Ŀ����</param>
    /// <param name="height">Ŀ��߶�</param>
    /// <returns>����Ϸ�����lossyScaleӦ����Ϊʲô</returns>
    public static Vector3 ScaleWithScreen(SpriteRenderer spriteRenderer, float width, float height, SpriteFillMode fillMode)
    {
        if (spriteRenderer == null)
            return Vector3.one;
        GameObject obj = spriteRenderer.gameObject;
        if (obj == null)
            return Vector3.one;
        Vector3 origin = obj.transform.localScale;
        Sprite sprite = spriteRenderer.sprite;
        if (sprite == null)
            return origin;

        float kx, ky;
        kx = width == 0 ? 1f :
        ky = height == 0 ? 1f : CAMERA_HEIGHT * 100 / sprite.rect.height * height;
        switch (fillMode)
        {
            case WidthOnly:
                kx = CAMERA_WIDTH * 100 / sprite.rect.width * width;
                ky = origin.y;
                break;
            case HeightOnly:
                kx = origin.x;
                ky = CAMERA_HEIGHT * 100 / sprite.rect.height * height;
                break;
            case Free:
                kx = CAMERA_WIDTH * 100 / sprite.rect.width * width;
                ky = CAMERA_HEIGHT * 100 / sprite.rect.height * height;
                break;
            case ByWidthEqually:
                kx = CAMERA_WIDTH * 100 / sprite.rect.width * width;
                ky = kx;
                break;
            case ByHeightEqually:
                ky = CAMERA_HEIGHT * 100 / sprite.rect.height * height;
                kx = ky;
                break;
            default:
                kx = origin.x;
                ky = origin.y;
                break;
        }
        return new Vector3(kx, ky, 1f);
    }
}
