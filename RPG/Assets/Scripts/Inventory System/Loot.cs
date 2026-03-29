using Entity.Player;
using Item;
using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
    public class Loot : MonoBehaviour, IInteractable
    {
        private List<ItemInventory> _items;

        private LootFactory _factory;

        public bool CanInteract()
        {
            return gameObject.activeSelf;
        }

        public void Interact(PlayerController playerController)
        {
            if (!CanInteract()) return;

            foreach(var item in _items)
            {
                Debug.Log(item.Amount);

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
