using ObjectPool;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private EventSystem eventSystem;

    private void Awake()
    {
        eventSystem = ServiceLocator.GetService<EventSystem>();
    }

}
