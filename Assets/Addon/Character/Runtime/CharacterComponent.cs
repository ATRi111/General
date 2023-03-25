using UnityEngine;

namespace Character
{
    public abstract class CharacterComponent : MonoBehaviour
    {
        [AutoComponent(EComponentPosition.SelfOrParent)]
        protected CharacterEntity entity;

        protected virtual void Awake()
        {
            AutoComponentAttribute.Apply(this);
        }
    }
}