using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextBox : MonoBehaviour
{
    private Text m_text;

    [Header("Ã¿Ãë×ÖÊý")]
    [SerializeField]
    private float letterPerSeond = 5;

    private TypeWriter typeWriter;

    private void Awake()
    {
        m_text = GetComponent<Text>();
        typeWriter = new TypeWriter();
        ShowText("Hello World");
    }

    public void ShowText(string text,bool immediate = false)
    {
        typeWriter.Initialize(text,letterPerSeond);
        if (immediate)
            typeWriter.Complete();
    }

    private void Update()
    {
        if(!typeWriter.Paused || typeWriter.JustCompleted)
            m_text.text = typeWriter.Current;
    }
}
