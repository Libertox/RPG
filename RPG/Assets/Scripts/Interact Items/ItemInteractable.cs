using Entity.Player;
using InventorySystem;
using InteractionSystem;
using UnityEngine;

namespace QuestSystem
{
    public class ItemInteractable : InteractionBase
    {
        [SerializeField] private ItemConfigBase itemBase;

        public override void Execute(PlayerController playerController)
        {
            base.Execute(playerController);

            playerController.PlayerInventory.InventoryStorage.AddItemAndNotify(itemBase);

            ///Change to ppol
            Destroy(gameObject);
        }
    }
}
