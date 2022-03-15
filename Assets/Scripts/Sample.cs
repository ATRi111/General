using ObjectPool;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private AudioPlayer audioPlayer;
    private ObjectManager objectManager;
    private MyAudioSource audioSource;

    private void Awake()
    {
        Data data = new Data();
        JsonTool.SaveAsJson(data,FileTool.StreamingAssetsPath("new.json"));
    }
}

public class Data
{
    public int a;
    public string b;
    public GameObject c;

    public Data()
    {
        a = Random.Range(0, 10);
        b = Random.Range(0, 1).ToString();
    }
}