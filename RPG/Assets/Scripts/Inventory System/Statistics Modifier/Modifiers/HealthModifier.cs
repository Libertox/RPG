using Entity;
using Utility;

namespace InventorySystem.StatisticModifier
{
    public class HealthModifier : StatisticModifier
    {
        public HealthModifier(ModifierType modifierType, float modifierValue) : base(modifierType, modifierValue)
        {
   
        }

        public override void Apply(EntityStatistic entityStatistic)
        {
            originalValue = entityStatistic.Health.Value;

            float modifiedValue = valueModifier.Apply(entityStatistic.Health.Value, modifierValue);

            entityStatistic.Health.Set(modifiedValue);
        }

        public override void Revert(EntityStatistic entityStatistic)
        {
            float modifiedValue = valueModifier.Apply(originalValue, modifierValue);

            float distinction = modifiedValue - originalValue;

            entityStatistic.Health.Subtract(distinction);
        }
    }
}
