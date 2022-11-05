using UnityEngine;
using static Tools.SpriteScaleMode;

namespace Tools
{
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
        /// <summary>
        /// ����Sprite�Ŀ��
        /// </summary>
        /// <param name="width">Ŀ���ȣ�����Ļ��ȵı����ƣ�</param>
        /// <param name="height">Ŀ��߶ȣ�����Ļ�߶ȵı����ƣ�</param>
        /// <param name="scaleMode">�α�ģʽ</param>
        /// <returns>����Ϸ�����lossyScaleӦ����Ϊ����</returns>
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
        /// ����ͼƬ��߱�
        /// </summary>
        public static float GetAspectRatio(Sprite sprite)
        {
            return sprite.rect.width / sprite.rect.height;
        }
    }
}
