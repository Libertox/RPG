using Entity;
using InventorySystem.StatisticModifier;
using System.Collections.Generic;

namespace InventorySystem
{
    public class InventorySlot
    {
        public ItemConfigBase ItemBase { get; }
        public List<IStatisticModifier> statisticModifiers;
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
            if (itemBase is IStatModifierProvider statModifierProvider)
            {
                statisticModifiers.Add(statModifierProvider.MainStatisticModifier.Create());

                foreach (var modifier in statModifierProvider.AdditionalStatisticModifiers)
                {
                    statisticModifiers.Add(modifier.Create());
                }
            }
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
