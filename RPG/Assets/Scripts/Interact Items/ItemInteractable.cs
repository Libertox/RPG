using Entity.Player;
using InventorySystem;
using InteractionSystem;
using UnityEngine;

namespace QuestSystem
{
    public class ItemInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemConfigBase itemBase;

        public bool CanInteract()
        {
            return true;
        }

        public void Interact(PlayerController playerController)
        {
            if (!CanInteract()) return;

            playerController.PlayerInventory.InventoryStorage.AddItemAndNotify(itemBase);

            ///Change to ppol
            Destroy(gameObject);
        }
    }
}
