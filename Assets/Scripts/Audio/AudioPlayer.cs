using UnityEngine;

//要使用此类，先用导入的音频创建游戏物体，在游戏物体上挂载MyAudioSource脚本
//然后根据加载资源的方式，在对应的文件夹里创建预制体
public class AudioPlayer : Service
{
    protected AssetLoader assetLoader;

    private void Start()
    {
        assetLoader = Get<AssetLoader_Resoureces>();
    }

    /// <summary>
    /// 创建音源（游戏物体）
    /// </summary>
    /// <param name="path">音源所在的prefab的路径</param>
    /// <param name="transform">将音源设为transform的子物体，可以为null</param>
    /// <param name="play">是否立刻播放音频</param>
    public MyAudioSource CreateAudio(string path, Transform transform = null, bool play = true)
    {
        GameObject obj;
        MyAudioSource myAudioSource = null;

        void AfterLoad(GameObject asset)
        {
            obj = Instantiate(asset);
            myAudioSource = obj.GetComponent<MyAudioSource>();
            if (myAudioSource == null)
            {
                Debug.LogWarning("创建的游戏物体未挂载MyAudioSource脚本");
                return;
            }
            if (transform != null)
                obj.transform.parent = transform;
            obj.transform.position = Vector3.zero;
            if (play)
                myAudioSource.Play();
            else
                myAudioSource.Stop();
        }

        if (transform == null)
            transform = this.transform;
        assetLoader.LoadAsset<GameObject>(path, AfterLoad, false);  //如果不同步加载，无法返回MyAudioSource
        return myAudioSource;
    }
}
