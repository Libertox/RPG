
namespace Entity
{
    public class AttackState : BaseState
    {
        private readonly IAnimationController animationController;
        private readonly ICombatController combatController;

        public AttackState(EntityController entityController) : base(entityController)
        {
            animationController = entityController.GetController<IAnimationController>();
            combatController = entityController.GetController<ICombatController>();
        }

        public override void OnEnter()
        {
            animationController.SetAttackAnimation();

            combatController.Attack();

            animationController.WaitForEndAnimation(AnimationName.ATTACK, OnAttackAnimationComplete);      
        }

        public void OnAttackAnimationComplete()
        {
            combatController.SetIsAttacking(false);
        }

    }
}
