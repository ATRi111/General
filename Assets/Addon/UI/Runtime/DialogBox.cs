using MyTimer;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class DialogBox : TextBase
{
    [SerializeField]
    private int letterPerSceond = 15;
    [SerializeField]
    private float interval = 0.5f;

    public TypeWriter TypeWriter { get; private set; }
    public TypeWriterExtend TypeWriterExtend { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        TypeWriter = new TypeWriter();
        TypeWriter.OnTick += OnUpdate;
        TypeWriter.AfterCompelete += OnUpdate;
        TypeWriterExtend = new TypeWriterExtend();
        TypeWriterExtend.Initialize(TypeWriter, interval);
    }

    public void ShowText(string text, bool immediate = false)
    {
        if (string.IsNullOrEmpty(text))
            TextUI.text = string.Empty;
        TypeWriter.Initialize(text, letterPerSceond);
        if (immediate)
            TypeWriter.ForceComplete();
    }

    private void OnUpdate(string value)
    {
        TextUI.text = value;
    }
}
