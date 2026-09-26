using Entity.Player;
using UnityEngine;
using Area;

namespace Entity.Enemy
{
    public class PatrolState : BaseState
    {
        private readonly EnemyController _enemyController;
        private readonly PatrolArea _patrolArea;

        public PatrolState(EntityController entityController) : base(entityController)
        {
            _enemyController = (EnemyController)entityController;
            _patrolArea = _enemyController.PatrolArea;
        }

        public override void OnEnter()
        {
            Vector3 patrolPosiiton = _patrolArea.GetRandomPositionWithin();

            _enemyController.SetDestination(patrolPosiiton);

            _entityController.GetController<IAnimationController>().SetMoveAnimation();
        }

        public override void Update()
        {
            if(_enemyController.IsOnDestination())
                _enemyController.SetPatroling(false);
        }

    }
}
