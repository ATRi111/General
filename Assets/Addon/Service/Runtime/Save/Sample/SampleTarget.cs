using UnityEngine;

namespace Services.Save
{
    public class SampleTarget : MonoBehaviour
    {
        private SaveGroupController controller;
        public int[] data1;
        public float data2;
        public string data3;

        private void Update()
        {
            if(Input.GetKeyUp(KeyCode.S))
            {
                controller.Save();
            }
            if(Input.GetKeyUp(KeyCode.L))
            {
                controller.Load();
            }
        }
    }
}