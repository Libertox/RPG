using Entity.Player;
using InventorySystem;
using Item;
using UnityEngine;

namespace QuestSystem
{
    public class ItemInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private ItemBase itemBase;

        public bool CanInteract()
        {
            return true;
        }

        public void Interact(PlayerController playerController)
        {
            if (!CanInteract()) return;

            playerController.PlayerData.Inventory.AddItemAndUpdateInventory(itemBase);

            ///Change to ppol
            Destroy(gameObject);
        }
    }
}
