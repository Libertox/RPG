

using Entity;

namespace Entity.Player
{
    public class LocomotionState : BaseState
    {
        private readonly IMotionController _motionController;

        public LocomotionState(EntityController entityController, IMotionController motionController) : base(entityController)
        {
            _motionController = motionController;
        }

        public override void OnEnter()
        {
            _entityController.AnimationController.SetMoveAnimation();
        }

        public override void Update()
        {
            _motionController.Move();
        }

    }
}
