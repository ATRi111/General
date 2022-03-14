using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : Service
{
    protected override void Awake()
    {
        base.Awake();

    }

    /// <summary>
    /// 创建音源（游戏物体）
    /// </summary>
    /// <param name="path">音源路径</param>
    /// <param name="transform">将音源设为transform的子物体，可以为null</param>
    /// <returns></returns>
    public AudioSource CreateAudio(string path,Transform transform)
    {
        
        return null;
    }

    public void DestroyAudio()
    {

    }

    public void GetAudio()
    {

    }
}
