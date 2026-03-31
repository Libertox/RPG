

using Entity.Player;
using UnityEngine;

namespace InteractionSystem
{
    public class NPC : MonoBehaviour, IInteractable
    {
        public bool CanInteract()
        {
            return true;
        }

        public void Interact(PlayerController playerController)
        {
            if (!CanInteract()) return;

            Debug.Log("Interact");
        }
    }
}
