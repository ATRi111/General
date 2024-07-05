using Services;
using Services.Event;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public abstract class SliderBase<T> : MonoBehaviour
{
    protected IEventSystem eventSystem;
    public Slider Slider { get; protected set; }

    protected virtual void Awake()
    {
        eventSystem = ServiceLocator.Get<IEventSystem>();
        Slider = GetComponent<Slider>();
        Slider.onValueChanged.AddListener(OnValueChanged);
    }

    protected abstract void OnValueChanged(float value);

    protected abstract T ValueToData(float value);
    protected abstract float DataToValue(T data);
}
