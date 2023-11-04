using UnityEngine;

namespace Services.Save
{
    public class SampleTarget : MonoBehaviour
    {
        private SaveTargetController controller;
        public int[] data1;
        public float data2;
        public string data3;

        private void Awake()
        {
            controller = GetComponent<SaveTargetController>();
        }

        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.S))
            {
                controller.Group.Save();
            }
            if (Input.GetKeyUp(KeyCode.L))
            {
                controller.Group.Load();
            }
        }
    }
}