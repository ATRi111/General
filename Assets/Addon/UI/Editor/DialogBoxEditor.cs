using EditorExtend;
using UnityEditor;

[CustomEditor(typeof(DialogBox))]
public class DialogBoxEditor : AutoEditor
{
    [AutoProperty]
    public SerializedProperty letterPerSceond, interval;

    protected override void MyOnInspectorGUI()
    {
        letterPerSceond.IntSlider("每秒字数", 1, 50);
        interval.Slider("句子间隔", 0f, 2f);
    }
}