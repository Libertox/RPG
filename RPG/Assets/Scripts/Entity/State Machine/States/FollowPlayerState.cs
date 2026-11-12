
namespace Entity.Enemy
{
    public class FollowPlayerState : BaseState
    {
        private readonly EnemyController enemyController;

        public FollowPlayerState(EntityController entityController) : base(entityController)
        {
            enemyController = (EnemyController)entityController;
        }

        public override void OnEnter()
        {
            enemyController.MoveTowardsTarget();

            if (!enemyController.IsOnDestination())
                enemyController.GetController<IAnimationController>().SetMoveAnimation();
        }

        public override void Update()
        {
            enemyController.MoveTowardsTarget();
  
            if (enemyController.IsOnDestination())
            {
                enemyController.RotateTowardsTarget();

                if (enemyController.IsTargetAlive())
                    enemyController.GetController<ICombatController>().SetIsAttacking(true);
            }
        }
    }
}
