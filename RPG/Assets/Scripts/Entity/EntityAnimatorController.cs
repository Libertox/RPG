using System;
using System.Collections;
using UnityEngine;

namespace Entity
{
    public class EntityAnimatorController : IAnimationController
    {
        protected readonly int _idleAnimationHash = Animator.StringToHash(AnimationName.IDLE);
        protected readonly int _moveAnimationHash = Animator.StringToHash(AnimationName.MOVE);
        protected readonly int _attackAnimationHash = Animator.StringToHash(AnimationName.ATTACK);
        protected readonly int _takeDamageAnimationHash = Animator.StringToHash(AnimationName.TAKE_DAMAGE);
        protected readonly int _dieAnimationHash = Animator.StringToHash(AnimationName.DIE);

        protected readonly int _randomParameterHash = Animator.StringToHash("Random");

        public bool IsWaitingForEndAnimation { get; private set; }

        protected readonly Animator _animator;
        protected readonly float _crossFadeDuration = 0.2f;

        public EntityAnimatorController(Animator animator)
        {
            _animator = animator;
        }

        public virtual void SetIdleAnimation()
        {
            _animator.CrossFade(_idleAnimationHash, _crossFadeDuration);
        }

        public virtual void SetMoveAnimation()
        {
            _animator.CrossFade(_moveAnimationHash, _crossFadeDuration);
        }

        public virtual void SetAttackAnimation() 
        {
            _animator.CrossFade(_attackAnimationHash, _crossFadeDuration);
        }

        public virtual void SetGetHitAnimation()
        {
            _animator.CrossFade(_takeDamageAnimationHash, _crossFadeDuration);
        }

        public virtual void SetDieAnimation()
        {
            _animator.CrossFade(_dieAnimationHash, _crossFadeDuration); 
        }
       

        public IEnumerator WaitForEndAnimation(string stateName, Action OnAnimationComplete = null, int layer = 0)
        {
            IsWaitingForEndAnimation = true;

            while (!_animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
                yield return null;


            yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(layer).length);

            /*while (_animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1.0f)
                 yield return null;*/

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
