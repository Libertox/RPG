

namespace Player
{
    public class IdleState : BaseState
    {
        public IdleState(IAnimationController animationController) : base(animationController)
        {

        }

        public override void OnEnter()
        {
            _animationController.SetIdleAnimation();
        }
    }
}
