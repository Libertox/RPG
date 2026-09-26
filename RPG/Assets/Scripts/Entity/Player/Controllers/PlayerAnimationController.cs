
namespace Entity.Player
{
    public class PlayerAnimationController : EntityAnimatorController
    {
        private readonly int _attackAnimationCount = 3;

        public override void SetAttackAnimation()
        {
            int randomIndex = UnityEngine.Random.Range(0, _attackAnimationCount);

            animator.SetFloat(randomParameterHash, randomIndex);

            animator.CrossFade(attackAnimationHash, crossFadeDuration);
        }
    }

  
}
