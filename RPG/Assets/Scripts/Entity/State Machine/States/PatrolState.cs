using Entity.Player;
using UnityEngine;
using Area;

namespace Entity.Enemy
{
    public class PatrolState : BaseState
    {
        private readonly EnemyController enemyController;
        private readonly PatrolArea patrolArea;

        public PatrolState(EntityController entityController) : base(entityController)
        {
            enemyController = (EnemyController)entityController;
            patrolArea = enemyController.PatrolArea;
        }

        public override void OnEnter()
        {
            Vector3 patrolPosiiton = patrolArea.GetRandomPositionWithin();

            enemyController.SetDestination(patrolPosiiton);

            entityController.GetController<IAnimationController>().SetMoveAnimation();
        }

        public override void Update()
        {
            if(enemyController.IsOnDestination())
                enemyController.SetPatroling(false);
        }

    }
}
