using Entity.Player;
using InventorySystem;
using InteractionSystem;
using UnityEngine;

namespace QuestSystem
{
    public class ItemInteractable : InteractionBase
    {
        [SerializeField] private ItemConfigBase itemBase;

        [SerializeField] private int amount = 1;

        public override void Execute(PlayerController playerController)
        {
            base.Execute(playerController);

            playerController.PlayerInventory.InventoryStorage.AddItemAndNotify(new InventorySlot(itemBase, amount));

            ///Change to ppol
            Destroy(gameObject);
        }
    }
}
