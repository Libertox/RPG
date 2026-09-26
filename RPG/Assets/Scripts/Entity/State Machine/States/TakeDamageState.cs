

namespace Entity
{
    public class TakeDamageState : BaseState
    {
        private readonly IDamageable _damageable;

        public TakeDamageState(EntityController entityController) : base(entityController)
        {
            _damageable = entityController as IDamageable;
        }

        public override void OnEnter()
        {
            IAnimationController animationController = _entityController.GetController<IAnimationController>();

            animationController.SetGetHitAnimation();

            animationController.WaitForEndAnimation(AnimationName.TAKE_DAMAGE, OnAnimationComplete);
        }


        private void OnAnimationComplete()
        {
            _damageable?.SetTakeDamge(false);
        }
    }
}
