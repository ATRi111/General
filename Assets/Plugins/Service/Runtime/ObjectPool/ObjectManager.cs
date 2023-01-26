using UnityEngine;

namespace Services.ObjectPools
{
    public sealed class ObjectManager : Service, IObjectManager
    {
        private ObjectManagerCore core;
        private ObjectLocatorBase locator;

        protected internal override void Init()
        {
            base.Init();
            locator = new ObjectLocatorBase();
            core = new ObjectManagerCore(this, locator);
        }

        public IMyObject Activate(string identifier, Vector3 position, Vector3 eulerAngles, Transform parent = null)
            => core.Activate(identifier, position, eulerAngles, parent);

        public IMyObject Activate(string identifier, Vector3 position, float eulerAngleZ = 0f, Transform parent = null)
            => core.Activate(identifier, position, new Vector3(0f, 0f, eulerAngleZ), parent);

        public void PreCreate(string identifier, int count)
            => core.PreCreate(identifier, count);
    }
}

