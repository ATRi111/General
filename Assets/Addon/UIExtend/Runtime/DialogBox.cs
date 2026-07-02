using MyTimer;
using TMPro;
using UnityEngine;

namespace UIExtend
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class DialogBox : TextBase
    {
        [SerializeField]
        private int letterPerSecond = 15;
        [SerializeField]
        private float interval = 0.5f;

        public TypeWriter TypeWriter { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            TypeWriter = new TypeWriter();
            TypeWriter.OnTick += OnUpdate;
            TypeWriter.AfterComplete += OnUpdate;
        }

        public void ShowText(string text, bool immediate = false)
        {
            if (string.IsNullOrEmpty(text))
                TextUI.text = string.Empty;
            TypeWriter.Initialize(text, letterPerSecond, interval);
            if (immediate)
                TypeWriter.ForceComplete();
        }

        private void OnUpdate(string value)
        {
            TextUI.text = value;
        }
    }
}