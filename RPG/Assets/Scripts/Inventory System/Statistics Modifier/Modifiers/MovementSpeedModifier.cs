using Entity;
using Utility;

namespace InventorySystem.StatisticModifier
{
    public class MovementSpeedModifier : StatisticModifier
    {
        public MovementSpeedModifier(ModifierType modifierType, float modifierValue) : base(modifierType, modifierValue)
        {

        }

        public override void Apply(EntityStatistic entityStatistic)
        {
            originalValue = entityStatistic.MovementSpeed.Value;

            var modifiedValue = valueModifier.Apply(entityStatistic.MovementSpeed.Value, modifierValue);

            entityStatistic.MovementSpeed.Set(modifiedValue);
        }

        public override void Revert(EntityStatistic entityStatistic)
        {
            float modifiedValue = valueModifier.Apply(originalValue, modifierValue);

            float distinction = modifiedValue - originalValue;

            entityStatistic.MovementSpeed.Subtract(distinction);
        }
    }


}
