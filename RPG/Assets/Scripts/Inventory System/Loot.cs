using Entity.Player;
using InteractionSystem;
using System.Collections.Generic;

namespace InventorySystem
{
    public class Loot : InteractionBase
    {
        private List<InventorySlot> _items;

        private LootFactory _factory;

        public override void Execute(PlayerController playerController)
        {
            base.Execute(playerController);

            foreach (var item in _items)
            {
                playerController.PlayerInventory.InventoryStorage.AddItem(item);
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

        public void AddItem(InventorySlot item)
        {
            if (item == null) return;

            var findedItem = _items.Find((targetItem) => targetItem.ItemBase == item.ItemBase);

            if (findedItem != null)
            {
                findedItem.Amount += item.Amount;
            }
            else
            {
                _items.Add(new InventorySlot(item.ItemBase, item.Amount));
            }
        }
    }
}
