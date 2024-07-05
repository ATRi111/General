using Services;
using Services.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonBase : MonoBehaviour
{
    protected IEventSystem eventSystem;

    protected TextMeshProUGUI tmp;
    public Button Button { get; protected set; }

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
        tmp = GetComponentInChildren<TextMeshProUGUI>();
    }

    protected abstract void OnClick();
}
