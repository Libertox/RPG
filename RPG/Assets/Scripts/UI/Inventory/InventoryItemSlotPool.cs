using Zenject;

namespace UI.Inventory
{
    public class InventoryItemSlotPool : MonoMemoryPool<InventoryItemSlot>
    {
        protected override void OnCreated(InventoryItemSlot item)
        {
            
        }

        protected override void OnSpawned(InventoryItemSlot item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(InventoryItemSlot item)
        {
            item.gameObject.SetActive(false);
        }
    }
}
