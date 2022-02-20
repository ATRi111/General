using System.IO;
using UnityEngine;
using static SpriteFillMode;

public enum SpriteFillMode
{
    /// <summary>
    /// 宽高自由缩放
    /// </summary>
    Free,
    /// <summary>
    /// 仅改变宽度
    /// </summary>
    WidthOnly,
    /// <summary>
    /// 仅改变高度
    /// </summary>
    HeightOnly,
    /// <summary>
    /// 改变宽度，高度等比例缩放
    /// </summary>
    ByWidthEqually,
    /// <summary>
    /// 改变高度，宽度等比例缩放
    /// </summary>
    ByHeightEqually,
}

public static class SpriteTool
{
    public static Vector2 s_mid = new Vector2(0.5f, 0.5f);

    /// <summary>
    /// 加载一张图片，以原像素数显示（Pixel Per Unit为100）
    /// </summary>
    /// <param name="path">源文件路径</param>
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

    private const float CAMERA_HEIGHT = 10f;    //相机宽度（以unit计，修改相机大小后，这里也需要手动修改）
    private const float CAMERA_WIDTH = 17.78f;  //相机高度（以unit计，修改相机大小后，这里也需要手动修改）


    /// <summary>
    /// 调整图片的宽高
    /// </summary>
    /// <param name="width">目标宽度</param>
    /// <param name="height">目标高度</param>
    /// <returns>该游戏物体的lossyScale应调整为什么</returns>
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
