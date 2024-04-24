using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(CanvasGrounpPlus))]
public class CanvasGrounpPlusEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty fadeTime, immediate_next, immediate, threshold_blockRaycast, visibleOnAwake;

    protected override void MyOnInspectorGUI()
    {
        visibleOnAwake.BoolField("初始时可见");
        threshold_blockRaycast.Slider("Raycast阈值", 0f, 1f);
        immediate.BoolField("跳过渐变");
        if(!immediate.boolValue)
        {
            immediate_next.BoolField("跳过渐变(仅一次)");
            fadeTime.Slider("渐变时间", 0.1f, 10f);
        }
    }
}