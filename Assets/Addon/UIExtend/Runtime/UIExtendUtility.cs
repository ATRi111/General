using UnityEngine;

namespace UIExtend
{
    public static class UIExtendUtility
    {
        /// <summary>
        /// 判断某个RectTransform是否完全位于屏幕范围内(只能用于ScreenSpace-Overlay)
        /// </summary>
        /// <param name="padding">空隙(用屏幕宽度的百分比表示)</param>
        public static bool WithinScreen(RectTransform rectTransform, float padding = 0.01f)
        {
            padding *= Screen.width;
            GetBorder(rectTransform, out float left, out float right, out float bottom, out float top);
            return left >= padding && right <= Screen.width - padding
                && bottom >= padding && top <= Screen.height - padding;
        }

        /// <summary>
        /// 将一点限制在屏幕范围内
        /// </summary>
        /// <param name="padding">空隙(用屏幕宽度的百分比表示)</param>
        public static Vector3 ClampInScreen(Vector3 point, float padding = 0.01f)
        {
            padding *= Screen.width;
            float x = point.x;
            float y = point.y;
            x = Mathf.Clamp(x, padding, Screen.width - padding);
            y = Mathf.Clamp(y, padding, Screen.height - padding);
            return new Vector3(x, y, point.z);
        }

        /// <summary>
        /// 将RectTransform限制在屏幕范围内(只能用于ScreenSpace-Overlay)
        /// </summary>
        /// <param name="padding">空隙(用屏幕宽度的百分比表示)</param>
        public static void ClampInScreen(RectTransform rectTransform, float padding = 0.01f)
        {
            padding *= Screen.width;
            float x = rectTransform.position.x;
            float y = rectTransform.position.y;
            GetBorder(rectTransform, out float left, out float right, out float bottom, out float top);
            x = Mathf.Clamp(x, padding + x - left, Screen.width - padding + x - right);
            y = Mathf.Clamp(y, padding + y - bottom, Screen.height - padding + y - top);
            rectTransform.position = new(x, y);
        }

        /// <summary>
        /// 获取rectTransform的四边界（世界坐标系）
        /// </summary>
        public static void GetBorder(RectTransform rectTransform, out float left, out float right, out float bottom, out float top)
        {
            Vector3[] coners = new Vector3[4];
            rectTransform.GetWorldCorners(coners);
            left = coners[0].x;
            bottom = coners[0].y;
            right = coners[2].x;
            top = coners[2].y;
        }
    }
}