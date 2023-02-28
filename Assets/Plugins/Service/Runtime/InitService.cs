using UnityEngine;

namespace Services
{
    //此脚本仅用于便捷地挂载Service组件
    public class InitService : MonoBehaviour
    {
        public string search;

        private void Awake()
        {
            Destroy(this);
        }
    }
}