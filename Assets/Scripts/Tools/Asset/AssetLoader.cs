using System;
using UnityEngine;

//·�����ܰ�����չ��
public abstract class AssetLoader :Service
{
    /// <summary>
    /// ������Դ
    /// </summary>
    /// <typeparam name="T">��Դ����</typeparam>
    /// <param name="path">·�������ܰ�����չ��</param>
    public abstract T LoadAsset<T>(string path) where T : UnityEngine.Object;

    /// <summary>
    /// �첽������Դ
    /// </summary>
    /// <typeparam name="T">��Դ����</typeparam>
    /// <param name="path">·�������ܰ�����չ��</param>
    /// <param name="callBack">�ص������������൱�ڷ���ֵ</param>
    public abstract void LoadAssetAsync<T>(string path,Action<T> callBack) where T : UnityEngine.Object;

    public abstract void UnLoadAsset<T>(T t) where T : UnityEngine.Object;
}
