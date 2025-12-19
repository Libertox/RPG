using UnityEngine;

namespace Entity.Player
{
    public class PlayerAnimationController : EntityAnimatorController
    {
        private readonly int _attackAnimationCount = 3;

        public PlayerAnimationController(Animator animator) : base(animator)
        {

        }

        public override void SetAttackAnimation()
        {
            int randomIndex = UnityEngine.Random.Range(0, _attackAnimationCount);

            _animator.SetFloat(_randomParameterHash, randomIndex);

            _animator.CrossFade(_attackAnimationHash, _crossFadeDuration);
        }
    }

  
}
