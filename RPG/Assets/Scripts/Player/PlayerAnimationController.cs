

using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : IAnimationController
    {
        private readonly int _idleAnimationHash = Animator.StringToHash("Idle");
        private readonly int _moveAnimationHash = Animator.StringToHash("Move");
        private readonly int _attackAnimationHash = Animator.StringToHash("Attack");

        private readonly float _crossFadeDuration = 0.2f;

        private Animator _animator;
        public Animator Animator => _animator;

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
            _animator.CrossFade(_attackAnimationHash, _crossFadeDuration);
        }

    }
}
