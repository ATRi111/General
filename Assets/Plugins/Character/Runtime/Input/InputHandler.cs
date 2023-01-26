using UnityEngine;

namespace Character
{
    public static class InputHandler
    {
        public static InputManager<InputEvent_Button> buttonEvents;
        public static InputManager<InputEvent_Axis> axisEvents;

        static InputHandler()
        {
            buttonEvents = new InputManager<InputEvent_Button>();
            axisEvents = new InputManager<InputEvent_Axis>();
            GameObject obj = new GameObject("InputHandler");
            obj.AddComponent<InputHandlerMono>();
        }
    }
}