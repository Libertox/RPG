
namespace Entity
{
    public class IdleState : BaseState
    {
        public IdleState(EntityController entityController) : base(entityController)
        {

        }

        public override void OnEnter()
        {
            entityController.GetController<IAnimationController>().SetIdleAnimation();
        }
    }
}
