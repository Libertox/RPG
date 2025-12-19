
namespace Entity
{
    public class DieState : BaseState
    {
        public DieState(EntityController entityController) : base(entityController)
        {

        }

        public override void OnEnter()
        {
            _entityController.AnimationController.SetDieAnimation();
        }

    }
}
