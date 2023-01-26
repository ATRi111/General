using UnityEngine;

namespace Character
{
    [DefaultExecutionOrder(-100)]
    public class CharacterEntity : MonoBehaviour
    {
        protected virtual void Awake()
        {
            AutoComponentAttribute.Apply(this);
        }
    }
}