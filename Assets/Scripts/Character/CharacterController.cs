using UnityEngine;

// ��Ҫ��ȡCharacter�Ľű��ɼ̳д���
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
