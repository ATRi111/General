using UnityEngine;

namespace Services
{
    //�˽ű������ڱ�ݵع���Service���
    public class InitService : MonoBehaviour
    {
        public string search;

        private void Awake()
        {
            Destroy(this);
        }
    }
}