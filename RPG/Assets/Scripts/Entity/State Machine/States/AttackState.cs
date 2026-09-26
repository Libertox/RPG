
namespace Entity
{
    public class AttackState : BaseState
    {
        private readonly IAnimationController _animationController;
        private readonly ICombatController _combatController;

        public AttackState(EntityController entityController) : base(entityController)
        {
            _animationController = entityController.GetController<IAnimationController>();
            _combatController = entityController.GetController<ICombatController>();
        }

        public override void OnEnter()
        {
            _animationController.SetAttackAnimation();

            _combatController.Attack();

            _animationController.WaitForEndAnimation(AnimationName.ATTACK, OnAttackAnimationComplete);      
        }

        public void OnAttackAnimationComplete()
        {
            _combatController.SetIsAttacking(false);
        }

    }
}
