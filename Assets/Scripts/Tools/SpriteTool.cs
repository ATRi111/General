using System;
using System.Collections;
using System.IO;
using UnityEngine;
using static SpriteScaleMode;

public enum SpriteScaleMode
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
    ByWidth,
    /// <summary>
    /// 改变高度，宽度等比例缩放
    /// </summary>
    ByHeight,
}

public static class SpriteTool
{
    private const float HEIGHT_CAMERA = 10f;    //相机高度 = 相机尺寸 * 2
    private const float WIDTH_CAMERA = 17.78f;  //相机宽度 = 相机宽度 * 相机宽高比

    /// <summary>
    /// 调整Sprite的宽高
    /// </summary>
    /// <param name="width">目标宽度（以屏幕宽度的倍数计）</param>
    /// <param name="height">目标高度（以屏幕高度的倍数计）</param>
    /// <param name="scaleMode">形变模式</param>
    /// <returns>该游戏物体的lossyScale应调整为多少</returns>
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
