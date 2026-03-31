using Entity.Player;
using InteractionSystem;
using System.Collections.Generic;

namespace InventorySystem
{
    public class Loot : InteractionBase
    {
        private List<ItemInventory> _items;

        private LootFactory _factory;

        public override void Interact(PlayerController playerController)
        {
            if (!CanInteract()) return;

            foreach(var item in _items)
            {
                playerController.PlayerData.Inventory.AddItem(item.ItemBase, item.Amount);
            }

            _items.Clear();
            _factory.ReleaseLoot(this);
        }

        public Loot Initalize(LootFactory factory)
        {
            _items ??= new();
            _factory = factory;
            return this;
        }

        public void AddItem(ItemInventory item)
        {
            if (item == null) return;

            var findedItem = _items.Find((targetItem) => targetItem.ItemBase == item.ItemBase);

            if (findedItem != null)
            {
                findedItem.Amount += item.Amount;
            }
            else
            {
                _items.Add(new ItemInventory(item.ItemBase, item.Amount));
            }
        }
    }
}
