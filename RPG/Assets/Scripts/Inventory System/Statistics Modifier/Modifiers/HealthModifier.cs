using Entity;
using Utility;

namespace InventorySystem.StatisticModifier
{
    public class HealthModifier : IStatisticModifier
    {
        private readonly IValueModifier valueModifier;
        private readonly float modifierValue;

        private float originalValue;

        public HealthModifier(ModifierType modifierType, float modifierValue)
        {
            valueModifier = ValueModifierFactory.Create(modifierType);
            this.modifierValue = modifierValue;
        }

        public void Apply(EntityStatistic entityStatistic)
        {
            originalValue = entityStatistic.Health.Value;

            float modifiedValue = valueModifier.Apply(entityStatistic.Health.Value, modifierValue);

            entityStatistic.Health.Set(modifiedValue);
        }

        public void Revert(EntityStatistic entityStatistic)
        {
            float modifiedValue = valueModifier.Apply(originalValue, modifierValue);

            float distinction = modifiedValue - originalValue;

            entityStatistic.Health.Subtract(distinction);
        }
    }
}
