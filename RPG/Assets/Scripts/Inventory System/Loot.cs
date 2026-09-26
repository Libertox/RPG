using Entity.Player;
using InteractionSystem;
using System.Collections.Generic;

namespace InventorySystem
{
    public class Loot : InteractionBase
    {
        private List<InventorySlot> items;

        private LootFactory factory;

        public override void Execute(PlayerController playerController)
        {
            base.Execute(playerController);

            foreach (var item in items)
            {
                playerController.PlayerInventory.InventoryStorage.AddItem(item);
            }

            items.Clear();
            factory.ReleaseLoot(this);
        }

        public Loot Initalize(LootFactory factory)
        {
            items ??= new();
            this.factory = factory;
            return this;
        }

        public void AddItem(InventorySlot item)
        {
            if (item == null) return;

            var findedItem = items.Find((targetItem) => targetItem.ItemBase == item.ItemBase);

            if (findedItem != null)
            {
                findedItem.Amount += item.Amount;
            }
            else
            {
                items.Add(new InventorySlot(item.ItemBase, item.Amount));
            }
        }
    }
}
