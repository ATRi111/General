using UnityEditor;
using UnityEngine;

namespace EditorExtend
{
    internal static class SceneViewUtility
    {
        /// <summary>
        /// Scene窗口坐标转世界坐标下的射线
        /// </summary>
        public static Ray SceneToWorldRay(Vector2 scene)
        {
            return HandleUtility.GUIPointToWorldRay(scene);
        }

        /// <summary>
        /// Scene窗口坐标转世界坐标
        /// </summary>
        public static Vector3 SceneToWorld(Vector2 scene)
        {
            return HandleUtility.GUIPointToWorldRay(scene).origin;
        }


        /// <summary>
        /// Scene窗口尺寸（用屏幕高度的倍数表示）转世界坐标下的尺寸
        /// </summary>
        public static float SceneToWorldSize(float size)
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