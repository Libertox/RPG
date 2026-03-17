
using UnityEngine;
using UnityEngine.Pool;

namespace UI.Inventory
{
    public class InventoryItemSlotFactory
    {
        private readonly InventoryItemSlot _prefab;
        private readonly ObjectPool<InventoryItemSlot> _pool;

        public InventoryItemSlotFactory(InventoryItemSlot prefab)
        {
            _prefab = prefab;

            _pool = new ObjectPool<InventoryItemSlot>(OnCreate, OnGet, OnRelease);
        }

        public InventoryItemSlot Create()
        {
            return _pool.Get();
        }

        public void Destory(InventoryItemSlot slot)
        {
            _pool.Release(slot);
        }

        private InventoryItemSlot OnCreate()
        {
            return Object.Instantiate(_prefab);
        }

        private void OnGet(InventoryItemSlot itemSlot)
        {
            itemSlot.gameObject.SetActive(true);
        }

        private void OnRelease(InventoryItemSlot itemSlot)
        {
            itemSlot.gameObject.SetActive(false);
        }

    }
}
