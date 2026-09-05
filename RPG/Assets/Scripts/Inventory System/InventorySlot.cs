namespace InventorySystem
{
    public class InventorySlot
    {
        public ItemConfigBase ItemBase { get; }
        public int Amount;

        public InventorySlot(ItemConfigBase itemBase, int amount)
        {
            ItemBase = itemBase;
            Amount = amount;
        }
    }
}
