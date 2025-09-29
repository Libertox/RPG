

using UnityEngine;

namespace Item
{
    public class NPC : MonoBehaviour, IInteractable
    {
        public bool CanInteract()
        {
            return true;
        }

        public void Interact()
        {
            if (!CanInteract()) return;

            Debug.Log("Interact");
        }
    }
}
