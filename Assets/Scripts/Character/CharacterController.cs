using UnityEngine;

// 需要获取Character的脚本可继承此类
public class CharacterController : MonoBehaviour
{
    protected Character character;
    protected EventSystem eventSystem;

    protected virtual void Awake()
    {
        character = GetComponent<Character>();
        eventSystem = ServiceLocator.GetService<EventSystem>();
    }
}
