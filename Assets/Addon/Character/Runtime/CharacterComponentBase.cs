using UnityEngine;

namespace Character
{
    public abstract class CharacterComponentBase : MonoBehaviour 
    {
        [AutoComponent(EComponentPosition.SelfOrParent)]
        protected CharacterEntity entity;

        protected virtual void Awake()
        {
            AutoComponentAttribute.Apply(this);
        }
    }
}