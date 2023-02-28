using UnityEngine;

namespace Character
{
    //ʹ��InputHandlerʱ�����Զ��������д˽ű�����Ϸ���壬��ֹ�ֶ����Ӵ˽ű�
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