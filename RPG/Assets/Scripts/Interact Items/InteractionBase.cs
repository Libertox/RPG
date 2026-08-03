using Entity.Player;
using InteractionPromptSystem;
using UnityEngine;

namespace InteractionSystem
{
    public abstract class InteractionBase : MonoBehaviour,  IInteractable, IInteractablePrompt
    {
        public virtual bool CanInteract()
        {
            return true;
        }

        public void Interact(PlayerController playerController)
        {
            if (!CanInteract()) return;

            Execute(playerController);
        }

        public virtual void Execute(PlayerController playerController)
        {

        }
    }
}
