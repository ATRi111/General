using UnityEngine;

namespace Character
{
    [DefaultExecutionOrder(-100)]
    public class EntityBase : MonoBehaviour
    {
        protected virtual void Awake()
        {
            AutoComponentAttribute.Apply(this);
        }
    }
}