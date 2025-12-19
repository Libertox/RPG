using Utility;

namespace Entity.Enemy
{
    public class WaitingState : BaseState
    {
        private readonly Timer _waitTimer;
        private readonly EnemyController _enemyController;

        public WaitingState(EntityController entityController, float waitTime) : base(entityController)
        {
            _waitTimer = new Timer(waitTime, OnTimerElapsed);
            _enemyController = (EnemyController)entityController;
        }

        public override void OnEnter()
        {
            _entityController.AnimationController.SetIdleAnimation();
            _waitTimer.Start();
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();
            _waitTimer.Tick();
        }

        private void OnTimerElapsed()
        {
            _enemyController.SetPatroling(true);
        }
    }
}
