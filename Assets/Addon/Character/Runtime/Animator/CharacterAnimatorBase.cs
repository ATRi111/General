using UnityEngine;

namespace Character
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimatorBase : MonoBehaviour
    {
        protected Animator animator;
        public AnimatorStateInfo CurrentInfo => animator.GetCurrentAnimatorStateInfo(0);
        public int CurrentID => CurrentInfo.shortNameHash;

        protected virtual void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }
    }
}