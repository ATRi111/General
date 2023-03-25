using System;

namespace Services.Asset
{
    public sealed class AssetLoader : Service, IAssetLoader
    {
        internal AssetLoaderCore core;

        protected override void Awake()
        {
            base.Awake();
            core = new AssetLoaderCore_Addressable();
        }

        public T Load<T>(string address) where T : UnityEngine.Object
        {
            try
            {
                return core.Load<T>(address);
            }
            catch (Exception e)
            {
                Debugger.LogError(e.ToString(), EMessageType.System);
                Debugger.LogError($"�޷�������Դ����Դ��ַΪ{address}", EMessageType.System);
                return null;
            }
        }

        public void LoadAsync<T>(string address, Action<T> callBack) where T : UnityEngine.Object
        {
            try
            {
                core.LoadAsync(address, callBack);
            }
            catch (Exception e)
            {
                Debugger.LogException(e, EMessageType.System);
                Debugger.LogError($"�޷�������Դ����Դ��ַΪ{address}", EMessageType.System);
            }
        }

        public void Release<T>(string address) where T : UnityEngine.Object
        {
            try
            {
                core.Release<T>(address);
            }
            catch (Exception e)
            {
                Debugger.LogException(e, EMessageType.System);
                Debugger.LogError($"�޷��ͷ���Դ����Դ��ַΪ{address}", EMessageType.System);
            }
        }
    }
}