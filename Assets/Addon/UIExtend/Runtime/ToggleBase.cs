using Services;
using Services.Event;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UIExtend
{
    [RequireComponent(typeof(Toggle))]
    public abstract class ToggleBase : MonoBehaviour
    {
        protected IEventSystem eventSystem;

        protected TextMeshProUGUI tmp;
        public Toggle Toggle { get; protected set; }

        protected virtual void Awake()
        {
            eventSystem = ServiceLocator.Get<IEventSystem>();
            Toggle = GetComponent<Toggle>();
            Toggle.onValueChanged.AddListener(OnToggle);
            tmp = GetComponentInChildren<TextMeshProUGUI>();
        }

        protected abstract void OnToggle(bool value);
    }
}
