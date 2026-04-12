using Entity.Player;
using UnityEngine;

namespace InteractionSystem
{
    public abstract class InteractionBase : MonoBehaviour,  IInteractable
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
