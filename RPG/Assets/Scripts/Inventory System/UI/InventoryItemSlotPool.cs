using Zenject;

namespace InventorySystem.UI
{
    public class InventoryItemSlotPool : MonoMemoryPool<InventorySlotUI>
    {
        protected override void OnCreated(InventorySlotUI item)
        {
            
        }

        protected override void OnSpawned(InventorySlotUI item)
        {
            item.gameObject.SetActive(true);
        }

        protected override void OnDespawned(InventorySlotUI item)
        {
            item.gameObject.SetActive(false);
        }
    }
}
