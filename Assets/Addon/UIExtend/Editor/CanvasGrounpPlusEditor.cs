using EditorExtend;
using UnityEditor;
using UnityEngine;

namespace UIExtend
{
    [CustomEditor(typeof(CanvasGroupPlus))]
    public class CanvasGrounpPlusEditor : AutoEditor
    {
        [AutoProperty]
        public SerializedProperty fadeTime, immediate_next, immediate, threshold_blockRaycast, visibleOnAwake, alpha_default;

        protected override void MyOnInspectorGUI()
        {
            visibleOnAwake.BoolField("初始时可见");
            threshold_blockRaycast.Slider("Raycast阈值", 0f, 1f);
            immediate.BoolField("跳过渐变");
            if (!immediate.boolValue)
            {
                immediate_next.BoolField("跳过渐变(仅一次)");
                fadeTime.FloatField("渐变时间");
            }
            if (Application.isPlaying)
            {
                alpha_default.FloatField("默认不透明度");
            }
        }
    }
}