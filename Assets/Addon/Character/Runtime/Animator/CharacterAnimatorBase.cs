using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimatorBase : CharacterComponentBase
    {
        [AutoComponent(EComponentPosition.Indeterminate)]
        protected Animator animator;
        public AnimatorStateInfo CurrentInfo => animator.GetCurrentAnimatorStateInfo(0);
        public int CurrentID => CurrentInfo.shortNameHash;

        protected override void Awake()
        {
            base.Awake();
            AutoHashAttribute.Apply(this);
        }
    }
}