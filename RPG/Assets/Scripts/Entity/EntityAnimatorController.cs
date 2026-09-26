using System;
using System.Collections;
using UnityEngine;

namespace Entity
{
    public class EntityAnimatorController : MonoBehaviour, IAnimationController
    {
        [SerializeField] protected Animator animator;

        protected readonly int idleAnimationHash = Animator.StringToHash(AnimationName.IDLE);
        protected readonly int moveAnimationHash = Animator.StringToHash(AnimationName.MOVE);
        protected readonly int attackAnimationHash = Animator.StringToHash(AnimationName.ATTACK);
        protected readonly int takeDamageAnimationHash = Animator.StringToHash(AnimationName.TAKE_DAMAGE);
        protected readonly int dieAnimationHash = Animator.StringToHash(AnimationName.DIE);

        protected readonly int randomParameterHash = Animator.StringToHash("Random");

        public bool IsWaitingForEndAnimation { get; private set; }

        protected readonly float crossFadeDuration = 0.2f;


        public virtual void SetIdleAnimation()
        {
            animator.CrossFade(idleAnimationHash, crossFadeDuration);
        }

        public virtual void SetMoveAnimation()
        {
            animator.CrossFade(moveAnimationHash, crossFadeDuration);
        }

        public virtual void SetAttackAnimation() 
        {
            animator.CrossFade(attackAnimationHash, crossFadeDuration);
        }

        public virtual void SetGetHitAnimation()
        {
            animator.CrossFade(takeDamageAnimationHash, crossFadeDuration);
        }

        public virtual void SetDieAnimation()
        {
            animator.CrossFade(dieAnimationHash, crossFadeDuration); 
        }
       
        public void WaitForEndAnimation(string stateName, Action OnAnimationComplete = null, int layer = 0)
        {
            StartCoroutine(WaitForEndAnimationRoutine(stateName, OnAnimationComplete, layer));
        }

        private IEnumerator WaitForEndAnimationRoutine(string stateName, Action OnAnimationComplete = null, int layer = 0)
        {
            IsWaitingForEndAnimation = true;

            while (!animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
                yield return null;

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(layer).length);

            OnAnimationComplete?.Invoke();

            yield return null;
            yield return new WaitForEndOfFrame();

            IsWaitingForEndAnimation = false;
        }
    }

    public class AnimationName
    {
        public const string IDLE = "Idle";
        public const string MOVE = "Move";
        public const string ATTACK = "Attack";
        public const string TAKE_DAMAGE = "GetHit";
        public const string DIE = "Die";
    }
}
