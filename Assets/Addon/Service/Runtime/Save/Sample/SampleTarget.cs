using UnityEngine;

namespace Services.Save
{
    public class SampleTarget : MonoBehaviour
    {
        private GroupController controller;
        public int[] data1;
        public float data2;
        public string data3;

        private void Awake()
        {
            controller = ServiceLocator.Get<ISaveManager>().GetGroup(SaveData_Sample.Id);
            controller.Bind<SaveData_Sample>(SaveData.DefineIdentifier_Default(gameObject), this);
        }

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