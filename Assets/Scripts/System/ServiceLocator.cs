using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class ServiceLocator : Singleton<ServiceLocator>
{
    private int NumOfService;       //�ܷ�����
    [SerializeField]
    private int _NumOfLoadedService;
    /// <summary>
    /// �Ѽ��صķ�����
    /// </summary>
    private int NumOfLoadedService
    {
        get => _NumOfLoadedService;
        set
        {
            if (value == NumOfService)
            {
                Debug.Log("����������");
                OnServiceLoaded?.Invoke();
                OnServiceLoaded = null;
            }
            _NumOfLoadedService = value;
        }
    }
    /// <summary>
    /// ���������ʱҪִ�еķ���(Ӧ����Start��ע��)
    /// </summary>
    public event Action OnServiceLoaded;

    private readonly Dictionary<EService, Service> serviceDict = new Dictionary<EService, Service>();

    protected override void Awake()
    {
        base.Awake();
        NumOfService = Enum.GetValues(typeof(EService)).Length;
        _NumOfLoadedService = 0;
    }

    /// <summary>
    /// ��ȡһ������
    /// </summary>
    /// <returns>���Ҫ����ȡ�ķ���ֵ���丸�࣬������Ӧָ��Ϊ�÷������������丸��</returns>
    public T GetService<T>(EService eService) where T : Service
    {
        if (!serviceDict.ContainsKey(eService))
        {
            Debug.LogWarning($"����δ����ӵ�����λ����,��������Ϊ{eService}");
            return null;
        }

        Service service = serviceDict[eService];
        if (!(service is T))
        {
            Debug.LogWarning($"�޷���ȡ��ȷ�ķ��񣬷��������Ϊ{service.GetType().Name}");
            return null;
        }
        return (T)serviceDict[eService];
    }

    public void AddService(EService eService, Service service)
    {
        serviceDict.Add(eService, service);
        NumOfLoadedService++;
    }
}
