using System;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-200)]
public class ServiceLocator : Singleton<ServiceLocator>
{
    private int NumOfService;       //总服务数
    [SerializeField]
    private int _NumOfLoadedService;
    /// <summary>
    /// 已加载的服务数
    /// </summary>
    private int NumOfLoadedService
    {
        get => _NumOfLoadedService;
        set
        {
            if (value == NumOfService)
            {
                Debug.Log("服务加载完成");
                OnServiceLoaded?.Invoke();
                OnServiceLoaded = null;
            }
            _NumOfLoadedService = value;
        }
    }
    /// <summary>
    /// 服务加载完时要执行的方法(应当在Start中注册)
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
    /// 获取一个服务
    /// </summary>
    /// <returns>如果要将获取的服务赋值给其父类，泛型仍应指定为该服务的类而不是其父类</returns>
    public T GetService<T>(EService eService) where T : Service
    {
        if (!serviceDict.ContainsKey(eService))
        {
            Debug.LogWarning($"服务未被添加到服务定位器中,服务名称为{eService}");
            return null;
        }

        Service service = serviceDict[eService];
        if (!(service is T))
        {
            Debug.LogWarning($"无法获取正确的服务，服务的类型为{service.GetType().Name}");
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
