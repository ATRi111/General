using UnityEngine;
using static Tools.SpriteScaleMode;

namespace Tools
{
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
        /// <summary>
        /// 调整Sprite的宽高
        /// </summary>
        /// <param name="width">目标宽度（以屏幕宽度的倍数计）</param>
        /// <param name="height">目标高度（以屏幕高度的倍数计）</param>
        /// <param name="scaleMode">形变模式</param>
        /// <returns>该游戏物体的lossyScale应调整为多少</returns>
        public static Vector3 ScaleWithScreen(Camera camera, SpriteRenderer spriteRenderer, float width, float height, SpriteScaleMode scaleMode = Free)
        {
            if (spriteRenderer == null || spriteRenderer.transform == null || spriteRenderer.sprite == null)
                throw new System.ArgumentNullException();

            Vector3 original = spriteRenderer.transform.localScale;
            Sprite sprite = spriteRenderer.sprite;

            float cameraHeight = camera.orthographicSize * 2;
            float cameraWidth = cameraHeight * Screen.width / Screen.height;

            float pixelsPerUnit = sprite.pixelsPerUnit;
            float kx, ky;
            switch (scaleMode)
            {
                case WidthOnly:
                    kx = cameraWidth * pixelsPerUnit / sprite.rect.width * width;
                    ky = original.y;
                    break;
                case HeightOnly:
                    kx = original.x;
                    ky = cameraHeight * pixelsPerUnit / sprite.rect.height * height;
                    break;
                case Free:
                    kx = cameraWidth * pixelsPerUnit / sprite.rect.width * width;
                    ky = cameraHeight * pixelsPerUnit / sprite.rect.height * height;
                    break;
                case ByWidth:
                    kx = cameraWidth * pixelsPerUnit / sprite.rect.width * width;
                    ky = kx;
                    break;
                case ByHeight:
                    ky = cameraHeight * pixelsPerUnit / sprite.rect.height * height;
                    kx = ky;
                    break;
                default:
                    kx = original.x;
                    ky = original.y;
                    break;
            }
            return new Vector3(kx, ky, 1f);
        }

        /// <summary>
        /// 计算图片宽高比
        /// </summary>
        public static float GetAspectRatio(Sprite sprite)
        {
            return sprite.rect.width / sprite.rect.height;
        }
    }
}
