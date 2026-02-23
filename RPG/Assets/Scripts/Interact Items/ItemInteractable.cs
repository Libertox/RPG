using Entity.Player;
using InventorySystem;
using Item;
using UnityEngine;
using Zenject;

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

            playerController.PlayerData.Inventory.AddItem(itemBase);

            ///Change to ppol
            Destroy(gameObject);
        }
    }
}
