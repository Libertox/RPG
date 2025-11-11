

namespace Player
{
    public class LocomotionState : BaseState
    {
        private readonly IMotionController _motionController;

        public LocomotionState(IAnimationController animationController, IMotionController motionController) : base(animationController)
        {
            _motionController = motionController;
        }

        public override void OnEnter()
        {
            _animationController.SetMoveAnimation();
        }

        public override void Update()
        {
            _motionController.Move();
        }

    }
}
