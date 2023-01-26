using UnityEngine;
using UnityEngine.Events;

namespace Character
{
    public class InputEvent_Button : InputEvent
    {
        private bool up;
        public bool Up
        {
            get => up;
            private set
            {
                up = value;
                if (value)
                    ButtonUp?.Invoke();
            }
        }

        private bool down;
        public bool Down
        {
            get => down;
            private set
            {
                down = value;
                if (value)
                    ButtonDown?.Invoke();
            }
        }

        private bool pressed;
        public bool Pressed
        {
            get => pressed;
            private set => pressed = value;
        }

        public UnityAction ButtonUp;
        public UnityAction ButtonDown;

        public InputEvent_Button() : base() { }

        public InputEvent_Button(string axesName) : base(axesName) { }

        public override void GetInput()
        {
            Up = Input.GetButtonUp(axesName);
            Down = Input.GetButtonDown(axesName);
            Pressed = Input.GetButton(axesName);
        }

        /// <summary>
        /// 获取一个监视按钮松开的HoldBool
        /// </summary>
        public HoldBool InspectButtonUp()
        {
            HoldBool holdBool = new HoldBool(Up);
            ButtonUp += holdBool.SetTrue;
            return holdBool;
        }

        /// <summary>
        /// 获取一个监视按钮按下的HoldBool
        /// </summary>
        public HoldBool InspectButtonDown()
        {
            HoldBool holdBool = new HoldBool(Down);
            ButtonDown += holdBool.SetTrue;
            return holdBool;
        }
    }
}