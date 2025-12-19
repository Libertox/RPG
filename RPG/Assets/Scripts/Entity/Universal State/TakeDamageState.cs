

namespace Entity
{
    public class TakeDamageState : BaseState
    {
        private IDamageable _damageable;

        public TakeDamageState(EntityController entityController) : base(entityController)
        {
            _damageable = entityController as IDamageable;
        }

        public override void OnEnter()
        {
            _entityController.AnimationController.SetGetHitAnimation();

            _entityController.StartCoroutine(_entityController.AnimationController.WaitForEndAnimation(AnimationName.TAKE_DAMAGE, OnAnimationComplete));
        }


        private void OnAnimationComplete()
        {
            _damageable.SetTakeDamge(false);
        }
    }
}
