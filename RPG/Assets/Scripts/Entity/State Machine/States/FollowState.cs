
namespace Entity.Enemy
{
    public class FollowState : BaseState
    {
        private readonly EnemyController enemyController;

        public FollowState(EntityController entityController) : base(entityController)
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

                enemyController.GetController<ICombatController>().SetIsAttacking(true);
            }
        }
    }
}
