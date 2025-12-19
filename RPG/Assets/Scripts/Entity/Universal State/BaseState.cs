
using StateMachines;

namespace Entity
{
    public abstract class BaseState : IState
    {
        protected EntityController _entityController;

        public BaseState(EntityController entityController)
        {
            _entityController = entityController;
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
