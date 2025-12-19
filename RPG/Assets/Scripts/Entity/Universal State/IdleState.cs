
namespace Entity
{
    public class IdleState : BaseState
    {
        public IdleState(EntityController entityController) : base(entityController)
        {

        }

        public override void OnEnter()
        {
            _entityController.AnimationController.SetIdleAnimation();
        }
    }
}
