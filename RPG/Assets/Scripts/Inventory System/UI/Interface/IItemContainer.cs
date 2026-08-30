

namespace InventorySystem
{
    public interface IItemContainer
    {
        public bool Drop(InventoryItem item);
        public InventoryItem Get();

    }
}
