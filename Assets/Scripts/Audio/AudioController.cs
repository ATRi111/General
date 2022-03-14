using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Service
{
    protected AssetLoader assetLoader;

    protected override void Awake()
    {
        base.Awake();
        assetLoader = ServiceLocator.Instance.GetService<AssetLoader_Resoureces>();
    }

    /// <summary>
    /// 创建音源（游戏物体）
    /// </summary>
    /// <param name="path">音源路径</param>
    /// <param name="transform">将音源设为transform的子物体，可以为null</param>
    /// <param name="play">是否立刻播放音频</param>
    public MyAudioSource CreateAudio(string path, Transform transform, bool play = true)
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

        assetLoader.LoadAsset<GameObject>(path, AfterLoad, false);  //如果不同步加载，就无法返回MyAudioSource
        return myAudioSource;
    }

    /// <summary>
    /// 查找MyAudioSource脚本
    /// </summary>
    /// <param name="transform">从这个transform及子物体中查找</param>
    /// <param name="name">MyAudioSource脚本挂载的游戏物体的名字，若为空，返回第一个</param>
    public MyAudioSource GetAudio(Transform transform,string name = null)
    {
        if(name == null)
            return transform.GetComponentInChildren<MyAudioSource>();
        return transform.Find(name).GetComponentInChildren<MyAudioSource>();
    }

    /// <summary>
    /// 强制摧毁transform下所有挂载MyAudioSource的子物体
    /// </summary>
    public void DestroyAudio(Transform transform)
    {
        MyAudioSource[] myAudioSources = transform.GetComponentsInChildren<MyAudioSource>();
        foreach (MyAudioSource source in myAudioSources)
        {
            Destroy(source.gameObject);
        }
    }
}
