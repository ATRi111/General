using UnityEngine;

namespace Character
{
    public class InputEvent_Axis : InputEvent
    {
        public float Value { get; private set; }
        public float RawValue { get; private set; }

        public InputEvent_Axis() : base() { }
        public InputEvent_Axis(string axesName) : base(axesName) { }

        public override void GetInput()
        {
            Value = Input.GetAxis(axesName);
            RawValue = Input.GetAxisRaw(axesName);
        }
    }
}