
namespace Entity.Enemy
{
    public class FollowState : BaseState
    {
        private readonly EnemyController _enemyController;

        public FollowState(EntityController entityController) : base(entityController)
        {
            _enemyController = (EnemyController)entityController;
        }

        public override void OnEnter()
        {
            _enemyController.MoveTowardsTarget();

            if (!_enemyController.IsOnDestination())
                _enemyController.GetController<IAnimationController>().SetMoveAnimation();
        }

        public override void Update()
        {
            _enemyController.MoveTowardsTarget();
  
            if (_enemyController.IsOnDestination())
            {
                _enemyController.RotateTowardsTarget();

                _enemyController.GetController<ICombatController>().SetIsAttacking(true);
            }
        }
    }
}
