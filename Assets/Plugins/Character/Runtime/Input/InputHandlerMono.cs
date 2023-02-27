using UnityEngine;

namespace Character
{
    //使用InputHandler时，会自动创建带有此脚本的游戏物体，禁止手动添加此脚本
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