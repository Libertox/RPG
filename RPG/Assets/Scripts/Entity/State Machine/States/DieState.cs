using UnityEngine;

namespace Entity
{
    public class DieState : BaseState
    {
        public DieState(EntityController entityController) : base(entityController)
        {

        }

        public override void OnEnter()
        {
            var animationController = entityController.GetController<IAnimationController>();

            animationController.SetDieAnimation();

            animationController.WaitForEndAnimation(AnimationName.DIE, () =>
            {
                Object.Destroy(entityController.gameObject);
            });
        }

    }
}
