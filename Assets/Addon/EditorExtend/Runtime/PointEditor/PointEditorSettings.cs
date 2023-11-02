using UnityEngine;

namespace EditorExtend.PointEditor
{
    public class PointEditorSettings : ScriptableObject
    {
        public float DefaultLineThickness;
        public float DefaultDotSize;

        public Color LineColor;
        public Color PointColor;
        public Color SelectedPointColor;
        public Color NewPointColor;

        /// <summary>
        /// 鼠标靠近到点的此范围内时，会选中点（屏幕坐标系下）
        /// </summary>
        public float HitPointDistance;
        /// <summary>
        /// 鼠标靠近到线的此范围内时，会选中线（屏幕坐标系下）
        /// </summary>
        public float HitLineDistance;
        /// <summary>
        /// 鼠标靠近到游戏物体的此范围内时，会保持选中游戏物体的状态（屏幕坐标系下）
        /// </summary>
        public float ContainHitObjectDistance;
        /// <summary>
        /// 鼠标靠近到游戏物体的此范围内时，会选中游戏物体（屏幕坐标系下）
        /// </summary>
        public float HitObjectDistance;

        public float DefaultDistance;
    }
}