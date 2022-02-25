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
    private const float HEIGHT_CAMERA = 10f;    //�����ȣ���unitΪ��λ���޸������С������Ҳ��Ҫ�ֶ��޸ģ�
    private const float WIDTH_CAMERA = 17.78f;  //����߶ȣ���unitΪ��λ���޸������С������Ҳ��Ҫ�ֶ��޸ģ�

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
    /// <param name="width">Ŀ����</param>
    /// <param name="height">Ŀ��߶�</param>
    /// <param name="fillMode">���ģʽ</param
    /// <param name="pixelPerUnit">ͼƬ��Sprite Editor�е�pixelPerUnit</param>
    /// <returns>����Ϸ�����lossyScaleӦ����Ϊ����</returns>
    public static Vector3 ScaleWithScreen(SpriteRenderer spriteRenderer, float width, float height, SpriteFillMode fillMode,int pixelPerUnit = 100)
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
        switch (fillMode)
        {
            case WidthOnly:
                kx = WIDTH_CAMERA * pixelPerUnit / sprite.rect.width * width;
                ky = origin.y;
                break;
            case HeightOnly:
                kx = origin.x;
                ky = HEIGHT_CAMERA * pixelPerUnit / sprite.rect.height * height;
                break;
            case Free:
                kx = WIDTH_CAMERA * pixelPerUnit / sprite.rect.width * width;
                ky = HEIGHT_CAMERA * pixelPerUnit / sprite.rect.height * height;
                break;
            case ByWidthEqually:
                kx = WIDTH_CAMERA * pixelPerUnit / sprite.rect.width * width;
                ky = kx;
                break;
            case ByHeightEqually:
                ky = HEIGHT_CAMERA * pixelPerUnit / sprite.rect.height * height;
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
