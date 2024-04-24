using EditorExtend;
using MyTimer;
using UnityEditor;

public class TimerIndirectEditor : IndirectEditor
{
    protected override string DefaultLabel => "计时器";
    [AutoProperty]
    public SerializedProperty paused, completed;
    protected ITimer timer;

    public TimerIndirectEditor(SerializedProperty serializedProperty, ITimer timer, string label = null) : base(serializedProperty, label)
    {
        this.timer = timer;
    }

    protected override void MyOnInspectorGUI()
    {
        EditorGUI.BeginDisabledGroup(true);
        paused.BoolField("计时暂停");
        completed.BoolField("计时完成");
        EditorGUILayout.FloatField("总时间", timer.Duration);
        EditorGUILayout.FloatField("当前时间", timer.Time);
        EditorGUI.EndDisabledGroup();
    }
}