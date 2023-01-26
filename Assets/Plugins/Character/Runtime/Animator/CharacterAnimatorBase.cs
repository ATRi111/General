using UnityEngine;

namespace Character
{
    public class CharacterAnimatorBase : CharacterComponent
    {
        [AutoComponent]
        private Animator animator;
        public AnimatorStateInfo CurrentInfo => animator.GetCurrentAnimatorStateInfo(0);
        public int CurrentID => CurrentInfo.shortNameHash;
    }
}