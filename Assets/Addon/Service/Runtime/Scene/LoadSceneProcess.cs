using UnityEngine.SceneManagement;

namespace Services
{
    internal class LoadSceneProcess
    {
        internal int index;
        internal LoadSceneMode mode;
        internal bool async;
        internal bool needConfirm;

        public LoadSceneProcess(int index, LoadSceneMode mode, bool async, bool needConfirm)
        {
            this.index = index;
            this.mode = mode;
            this.async = async;
            this.needConfirm = needConfirm;
        }
    }
}