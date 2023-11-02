using UnityEditor;

namespace EditorExtend
{
    internal static class SceneViewUtility
    {
        /// <summary>
        /// Scene窗口尺寸（用屏幕高度的倍数表示）转世界坐标下的尺寸
        /// </summary>
        public static float ScreenToWorldSize(float size)
        {
            return size * SceneView.currentDrawingSceneView.camera.orthographicSize * 2;
        }

        /// <summary>
        /// 世界坐标下的尺寸转Scene窗口尺寸（用屏幕高度的倍数表示）
        /// </summary>
        public static float WorldToSceneSize(float size)
        {
            return size / SceneView.currentDrawingSceneView.camera.orthographicSize / 2;
        }
    }
}