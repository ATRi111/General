using Services;
using Services.Event;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public abstract class ButtonBase : MonoBehaviour
{
    protected IEventSystem eventSystem;
    public Button Button { get; private set; }

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        Button = GetComponent<Button>();
        Button.onClick.AddListener(OnClick);
    }

    protected abstract void OnClick();
}
