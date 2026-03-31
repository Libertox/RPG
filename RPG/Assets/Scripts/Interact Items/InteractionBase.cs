using Entity.Player;

namespace InteractionSystem
{
    public abstract class InteractionBase : IInteractable
    {
        public virtual bool CanInteract()
        {
            return true;
        }

        public virtual void Interact(PlayerController playerController)
        {
          
        }
    }
}
