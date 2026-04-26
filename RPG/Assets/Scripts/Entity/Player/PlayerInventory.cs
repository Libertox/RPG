
using System;
using UnityEngine;
using static UnityEditor.Timeline.Actions.MenuPriority;

namespace InventorySystem
{
    public class PlayerInventory : MonoBehaviour
    {
        public event Action<ItemInventory> OnItemDropped;
        public event Action<int> OnGoldChanged;

        [field: SerializeField] public InventorySettings InventorySettings { get; private set; }
        public int Gold { get; private set; }

        private IInventoryStorage _inventoryStorage;
        private Equipment _equipment;

        public IInventoryStorage InventoryStorage => _inventoryStorage ??= new Inventory(); 
        public Equipment Equipment => _equipment ??= new(InventorySettings);

        private void Start()
        {
            _equipment.OnItemSwapped += OnItemSwapped;
            _equipment.OnItemEquipped += OnItemEquipped;
        }

        private void OnItemEquipped(ItemInventory item, int slot)
        {
            InventoryStorage.RemoveItemAndNotify(item.ItemBase);
        }

        private void OnItemSwapped(ItemInventory newItem, ItemInventory oldItem)
        {
            InventoryStorage.AddItem(oldItem.ItemBase, oldItem.Amount);
        }

        public void DropItem(ItemInventory item)
        {
            OnItemDropped?.Invoke(item);

            InventoryStorage.RemoveItemAndNotify(item.ItemBase);
        }

        public void AddGold(int amount)
        {
            Gold += amount;

            Debug.Log(amount + "gold added");

            OnGoldChanged?.Invoke(Gold);
        }

        public void RemoveGold(int amount)
        {
            Gold -= amount;

            if(Gold < 0)
                Gold = 0;

            Debug.Log(amount + "gold removed");

            OnGoldChanged?.Invoke(Gold);
        }
    }

    public class ItemInventory
    {
        public ItemConfigBase ItemBase;
        public int Amount;

        public ItemInventory(ItemConfigBase itemBase, int amount)
        {
            ItemBase = itemBase;
            Amount = amount;
        }
    }
}
