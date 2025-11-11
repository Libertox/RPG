
using StateMachines;

namespace Player
{
    public abstract class BaseState : IState
    {
        protected IAnimationController _animationController;

        public BaseState(IAnimationController animationController)
        {
            _animationController = animationController;
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void Update()
        {
            
        }

        public virtual void FixedUpdate()
        {

        }

        public virtual void OnExit()
        {

        }
    }
}
