using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class MyButton : MonoBehaviour
{
    protected Button m_button;

    protected void Awake()
    {
        m_button = GetComponent<Button>();
        m_button.onClick.AddListener(OnClick);
    }

    public abstract void OnClick(); 
}
