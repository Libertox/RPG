using Entity;
using Utility;

namespace InventorySystem.StatisticModifiers
{
    public class MovementSpeedModifier : StatisticModifier
    {
        public MovementSpeedModifier(StatisticModifierConfig statisticModifierConfig) : base(statisticModifierConfig)
        {

        }

        public override void Apply(EntityStatistic entityStatistic)
        {
            originalValue = entityStatistic.MovementSpeed.Value;

            var modifiedValue = valueModifier.Apply(entityStatistic.MovementSpeed.Value, config.ModifierValue);

            entityStatistic.MovementSpeed.Set(modifiedValue);
        }

        public override float GetModifiedValue(EntityStatistic entityStatistic)
        {
            return config.ModifierType switch
            {
                ModifierType.Addition => config.ModifierValue,
                ModifierType.Percentage => valueModifier.Apply(entityStatistic.EntityData.BaseMovementSpeed, config.ModifierValue) - entityStatistic.EntityData.BaseMovementSpeed,
                _ => 0,
            };
        }

        public override void Revert(EntityStatistic entityStatistic)
        {
            float modifiedValue = valueModifier.Apply(originalValue, config.ModifierValue);

            float distinction = modifiedValue - originalValue;

            entityStatistic.MovementSpeed.Subtract(distinction);
        }
    }


}
