using UnityEngine;

namespace Player
{
    public class PlayerAnimationController : IAnimationController
    {
        private readonly int _idleAnimationHash = Animator.StringToHash("Idle");
        private readonly int _moveAnimationHash = Animator.StringToHash("Move");
        private readonly int _attackAnimationHash = Animator.StringToHash("Attack");

        private readonly int _randomParameterHash = Animator.StringToHash("Random");

        private readonly float _crossFadeDuration = 0.2f;
        private readonly int _attackAnimationCount = 3;

        private readonly Animator _animator;

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
            int randomIndex = Random.Range(0, _attackAnimationCount);

            _animator.SetFloat(_randomParameterHash, randomIndex);

            _animator.CrossFade(_attackAnimationHash, _crossFadeDuration);
        }

        public AnimatorStateInfo GetCurrentAnimatorStateInfo() => _animator.GetCurrentAnimatorStateInfo(0);

   

    }
}
