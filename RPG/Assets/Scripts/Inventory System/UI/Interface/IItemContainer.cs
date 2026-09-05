

namespace InventorySystem
{
    public interface IItemContainer
    {
        public bool Drop(InventorySlot item);
        public InventorySlot Get();

    }
}
