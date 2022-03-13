using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetLoadSample : MonoBehaviour
{
    private AssetLoader assetLoader;

    private void Awake()
    {
        assetLoader = ServiceLocator.Instance.GetService<AssetLoader_Resoureces>();
        assetLoader.LoadAsset<GameObject>("AudioSource/Audio", AfterLoadAudio);
    }

    private void AfterLoadAudio(GameObject obj_audio)
    {
        Instantiate(obj_audio);
    }
}
