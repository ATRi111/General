using UnityEngine;

namespace Character
{
    public abstract class CharacterComponentBase : MonoBehaviour
    {
        [AutoComponent(EComponentPosition.SelfOrParent)]
        protected EntityBase entity;

        protected virtual void Awake()
        {
            AutoComponentAttribute.Apply(this);
        }
    }
}