
namespace Entity.Player
{
    public class LocomotionState : BaseState
    {
        private readonly IMotionController _motionController;

        public LocomotionState(EntityController entityController) : base(entityController)
        {
            _motionController = entityController.GetController<IMotionController>();
        }

        public override void OnEnter()
        {
            _entityController.GetController<IAnimationController>().SetMoveAnimation();
        }

        public override void Update()
        {
            _motionController.Move();
        }

    }
}
