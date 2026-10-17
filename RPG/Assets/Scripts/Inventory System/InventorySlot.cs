using Entity;
using InventorySystem.StatisticModifiers;
using System;
using System.Collections.Generic;

namespace InventorySystem
{
    public class InventorySlot
    {
        public ItemConfigBase ItemConfig { get; }  
        public int Amount;

        public StatisticModifier MainModifier { get; private set; }
        public List<StatisticModifier> AllModifiers { get; private set; }

        public InventorySlot(ItemConfigBase itemBase, int amount)
        {
            ItemConfig = itemBase;
            Amount = amount;
            AllModifiers = new();

            SetupStatisticModifers(itemBase);
        }

        private void SetupStatisticModifers(ItemConfigBase itemBase)
        {
            if (itemBase.MainStatisticModifier)
            {
                var statisticModifier = itemBase.MainStatisticModifier.Create();

                MainModifier = statisticModifier;
            }
             

            foreach (var modifier in itemBase.AdditionalStatisticModifiers)
            {
                AllModifiers.Add(modifier.Create());
            }
        }

        public void ApplyAllModifiers(EntityStatistic entityStatistic)
        {
            MainModifier?.Apply(entityStatistic);

            foreach (var modifier in AllModifiers)
                modifier.Apply(entityStatistic);
        }

        public void RevertAllModifiers(EntityStatistic entityStatistic)
        {
            MainModifier?.Revert(entityStatistic);

            foreach (var modifier in AllModifiers)
                modifier.Revert(entityStatistic);
        }

        public bool ContainModifierType(Type modifierType)
        {
            foreach (var modifier in AllModifiers)
            {
                if (modifier.GetType() == modifierType)
                    return true;
            }

            return false;
        }

        public StatisticModifier GetModifierByType(Type modifierType) 
        {
            foreach (var modifier in AllModifiers)
            {
                if (modifier.GetType() == modifierType)
                    return modifier;
            }

            return null;
        }
    }
}
