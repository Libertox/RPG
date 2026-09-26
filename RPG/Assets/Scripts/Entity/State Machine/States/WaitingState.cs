using Utility;

namespace Entity.Enemy
{
    public class WaitingState : BaseState
    {
        private readonly Timer waitTimer;
        private readonly EnemyController enemyController;

        public WaitingState(EntityController entityController, float waitTime) : base(entityController)
        {
            waitTimer = new Timer(waitTime, OnTimerElapsed);
            enemyController = (EnemyController)entityController;
        }

        public override void OnEnter()
        {
            entityController.GetController<IAnimationController>().SetIdleAnimation();
            waitTimer.Start();
            base.OnEnter();
        }

        public override void Update()
        {
            base.Update();
            waitTimer.Tick();
        }

        private void OnTimerElapsed()
        {
            enemyController.SetPatroling(true);
        }
    }
}
