using UnityEngine.SceneManagement;

namespace Services.SceneManagement
{
    internal struct LoadSceneRequest
    {
        internal int index;
        internal LoadSceneMode mode;
        internal bool async;
        internal bool needConfirm;

        public LoadSceneRequest(int index, LoadSceneMode mode, bool async, bool needConfirm)
        {
            this.index = index;
            this.mode = mode;
            this.async = async;
            this.needConfirm = needConfirm;
        }
    }
}