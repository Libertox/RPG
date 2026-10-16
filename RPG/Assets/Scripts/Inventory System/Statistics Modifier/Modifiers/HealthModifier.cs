using Entity;
using Utility;

namespace InventorySystem.StatisticModifiers
{
    public class HealthModifier : StatisticModifier
    {
        public HealthModifier(StatisticModifierConfig statisticModifierConfig) : base(statisticModifierConfig)
        {
   
        }

        public override void Apply(EntityStatistic entityStatistic)
        {
            originalValue = entityStatistic.Health.Value;

            float modifiedValue = valueModifier.Apply(entityStatistic.Health.Value, config.ModifierValue);

            entityStatistic.Health.Set(modifiedValue);
        }

        public override void Revert(EntityStatistic entityStatistic)
        {
            float modifiedValue = valueModifier.Apply(originalValue, config.ModifierValue);

            float distinction = modifiedValue - originalValue;

            entityStatistic.Health.Subtract(distinction);
        }

        public override float GetModifiedValue(EntityStatistic entityStatistic)
        {
            return config.ModifierType switch
            {
                ModifierType.Addition => config.ModifierValue,
                ModifierType.Percentage => valueModifier.Apply(entityStatistic.EntityData.BaseHealth, config.ModifierValue) - entityStatistic.EntityData.BaseHealth,
                _ => 0,
            };
        }
    }
}
