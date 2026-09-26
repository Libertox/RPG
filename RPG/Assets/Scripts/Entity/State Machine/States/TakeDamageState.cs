

namespace Entity
{
    public class TakeDamageState : BaseState
    {
        private readonly IDamageable damageable;

        public TakeDamageState(EntityController entityController) : base(entityController)
        {
            damageable = entityController as IDamageable;
        }

        public override void OnEnter()
        {
            IAnimationController animationController = entityController.GetController<IAnimationController>();

            animationController.SetGetHitAnimation();

            animationController.WaitForEndAnimation(AnimationName.TAKE_DAMAGE, OnAnimationComplete);
        }


        private void OnAnimationComplete()
        {
            damageable?.SetTakeDamge(false);
        }
    }
}
