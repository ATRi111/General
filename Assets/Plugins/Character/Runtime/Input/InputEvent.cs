using System;

namespace Character
{
    public abstract class InputEvent
    {
        public string axesName;

        public InputEvent()
        {
            axesName = "Undefined";
        }

        public InputEvent(string axesName)
        {
            this.axesName = axesName;
        }

        public abstract void GetInput();


        public override bool Equals(object obj)
        {
            return GetType().Equals(obj.GetType()) && axesName == ((InputEvent)obj).axesName;
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(axesName, GetType());
        }
        public static bool operator ==(InputEvent e1, InputEvent e2)
        {
            return e1.Equals(e2);
        }
        public static bool operator !=(InputEvent e1, InputEvent e2)
        {
            return !(e1 == e2);
        }
    }
}