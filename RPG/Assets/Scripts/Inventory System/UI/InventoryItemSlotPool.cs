using Zenject;

namespace InventorySystem.UI
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
