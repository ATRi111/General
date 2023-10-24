using Services.Asset;
using UnityEngine;

namespace Services
{
    public class ObjectLocatorBase : IObjectLocator
    {
        protected IAssetLoader assetLoader;

        public ObjectLocatorBase()
        {
            assetLoader = ServiceLocator.Get<IAssetLoader>();
        }

        public virtual GameObject Locate(string identifier)
        {
            return assetLoader.Load<GameObject>(identifier);
        }
    }
}