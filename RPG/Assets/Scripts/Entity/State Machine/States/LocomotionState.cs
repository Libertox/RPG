
namespace Entity.Player
{
    public class LocomotionState : BaseState
    {
        private readonly IMotionController motionController;

        public LocomotionState(EntityController entityController) : base(entityController)
        {
            motionController = entityController.GetController<IMotionController>();
        }

        public override void OnEnter()
        {
            entityController.GetController<IAnimationController>().SetMoveAnimation();
        }

        public override void Update()
        {
            motionController.Move();
        }

    }
}
