

using Entity.Player;

namespace Item
{
    public interface IInteractable
    {
        public bool CanInteract();
        public void Interact(PlayerController playerController);


    }
}
