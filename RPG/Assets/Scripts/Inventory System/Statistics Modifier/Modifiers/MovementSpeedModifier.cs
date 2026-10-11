using Entity;
using Utility;

namespace InventorySystem.StatisticModifier
{
    public class MovementSpeedModifier : IStatisticModifier
    {
        private readonly IValueModifier valueModifier;
        private readonly float modifierValue;

        private float originalValue;

        public MovementSpeedModifier(ModifierType modifierType, float modifierValue)
        {
            valueModifier = ValueModifierFactory.Create(modifierType);
            this.modifierValue = modifierValue;
        }

        public void Apply(EntityStatistic entityStatistic)
        {
            originalValue = entityStatistic.MovementSpeed.Value;

            var modifiedValue = valueModifier.Apply(entityStatistic.MovementSpeed.Value, modifierValue);

            entityStatistic.MovementSpeed.Set(modifiedValue);
        }

        public void Revert(EntityStatistic entityStatistic)
        {
            float modifiedValue = valueModifier.Apply(originalValue, modifierValue);

            float distinction = modifiedValue - originalValue;

            entityStatistic.MovementSpeed.Subtract(distinction);
        }
    }


}
