
namespace Player
{
    public class AttackState : BaseState
    {
        private readonly ICombatController _combatController;
        private readonly PlayerController _playerController;

        public AttackState(IAnimationController animationController, PlayerController playerController, ICombatController combatController) 
            : base(animationController)
        {
            _combatController = combatController;
            _playerController = playerController;
        }

        public override void OnEnter()
        {
            _animationController.SetAttackAnimation();

            _combatController.Attack();

            _playerController.StartCoroutine(_animationController.WaitForEndAnimation(AnimationName.ATTACK, OnAttackAnimationComplete));
        }

        public void OnAttackAnimationComplete()
        {
            _playerController.SetIsAttacking(false);
        }

    }
}
