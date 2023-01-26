using UnityEngine;

namespace Character
{
    //��ֹ�ֶ���Ӵ˽ű�
    internal class InputHandlerMono : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Update()
        {
            InputHandler.buttonEvents.Update();
            InputHandler.axisEvents.Update();
        }

        private void FixedUpdate()
        {
            InputHandler.buttonEvents.FiexdUpdate();
            InputHandler.axisEvents.FiexdUpdate();
        }
    }
}