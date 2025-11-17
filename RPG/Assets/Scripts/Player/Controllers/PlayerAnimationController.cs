using System;
using System.Collections;
using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : IAnimationController
    {
        private readonly int _idleAnimationHash = Animator.StringToHash(AnimationName.IDLE);
        private readonly int _moveAnimationHash = Animator.StringToHash(AnimationName.MOVE);
        private readonly int _attackAnimationHash = Animator.StringToHash(AnimationName.ATTACK);

        private readonly int _randomParameterHash = Animator.StringToHash("Random");

        private readonly float _crossFadeDuration = 0.2f;
        private readonly int _attackAnimationCount = 3;

        private readonly Animator _animator;

        public bool IsWaitingForEndAnimation { get; private set; }

        public PlayerAnimationController(Animator animator)
        {
            _animator = animator;
        }

        public void SetIdleAnimation()
        {
            _animator.CrossFade(_idleAnimationHash, _crossFadeDuration);
        }

        public void SetMoveAnimation()
        {
            _animator.CrossFade(_moveAnimationHash, _crossFadeDuration);
        }

        public void SetAttackAnimation()
        {
            int randomIndex = UnityEngine.Random.Range(0, _attackAnimationCount);

            _animator.SetFloat(_randomParameterHash, randomIndex);

            _animator.CrossFade(_attackAnimationHash, _crossFadeDuration);
        }

        public IEnumerator WaitForEndAnimation(string stateName, Action OnAnimationComplete = null, int layer = 0)
        {
            IsWaitingForEndAnimation = true;

            while (!_animator.GetCurrentAnimatorStateInfo(layer).IsName(stateName))
                yield return null;

            while (_animator.GetCurrentAnimatorStateInfo(layer).normalizedTime < 1.0f)
                yield return null;

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
    }
}
