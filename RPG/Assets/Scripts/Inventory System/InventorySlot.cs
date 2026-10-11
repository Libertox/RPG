using Entity;
using System.Collections.Generic;

namespace InventorySystem
{
    public class InventorySlot
    {
        public ItemConfigBase ItemBase { get; }
        public List<StatisticModifier.StatisticModifier> statisticModifiers;
        public int Amount;

        public InventorySlot(ItemConfigBase itemBase, int amount)
        {
            ItemBase = itemBase;
            Amount = amount;
            statisticModifiers = new();

            SetupStatisticModifers(itemBase);
        }

        private void SetupStatisticModifers(ItemConfigBase itemBase)
        {
            if (itemBase.MainStatisticModifier)
                statisticModifiers.Add(itemBase.MainStatisticModifier.Create());

            foreach (var modifier in itemBase.AdditionalStatisticModifiers)
            {
                statisticModifiers.Add(modifier.Create());
            }
        }

        public List<StatisticModifier.StatisticModifier> GetStatisticModifiers()
        {
            return statisticModifiers;
        }

        public void ApplyAllModifiers(EntityStatistic entityStatistic)
        {
            foreach (var modifier in statisticModifiers)
                modifier.Apply(entityStatistic);
        }

        public void RevertAllModifiers(EntityStatistic entityStatistic)
        {
            foreach (var modifier in statisticModifiers)
                modifier.Revert(entityStatistic);
        }
    }
}
