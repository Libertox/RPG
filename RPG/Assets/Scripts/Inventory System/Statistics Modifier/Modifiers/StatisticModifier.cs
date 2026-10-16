using Entity;
using Utility;

namespace InventorySystem.StatisticModifiers
{
    public abstract class StatisticModifier
    {
        protected readonly IValueModifier valueModifier;
        protected readonly StatisticModifierConfig config;

        public StatisticModifierConfig Config => config;

        protected float originalValue;

        public StatisticModifier(StatisticModifierConfig statisticModifierConfig)
        {
            this.valueModifier = ValueModifierFactory.Create(statisticModifierConfig.ModifierType);
          
            this.config = statisticModifierConfig;
        }

        public abstract float GetModifiedValue(EntityStatistic entityStatistic);
        public abstract void Apply(EntityStatistic entityStatistic);
        public abstract void Revert(EntityStatistic entityStatistic);
    }
}
