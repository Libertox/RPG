

using Entity.Player;

namespace InteractionSystem
{
    public interface IInteractable
    {
        public bool CanInteract();
        public void Interact(PlayerController playerController);


    }
}
