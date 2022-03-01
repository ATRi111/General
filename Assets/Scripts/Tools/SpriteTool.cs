using System;
using System.Collections;
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
