using Utility;
using Entity;

namespace InventorySystem.StatisticModifier
{
    public class AttackDamageModifier : IStatisticModifier
    {
        private readonly IValueModifier valueModifier;
        private readonly float modifierValue;

        private float originalValue;

        public AttackDamageModifier(ModifierType modifierType,  float modifierValue)
        {
            valueModifier = ValueModifierFactory.Create(modifierType);
            this.modifierValue = modifierValue;
        }

        public void Apply(EntityStatistic entityStatistic)
        {
            originalValue = entityStatistic.AttackDamage.Value;

            float modifiedValue = valueModifier.Apply(entityStatistic.AttackDamage.Value, modifierValue);

            entityStatistic.AttackDamage.Set(modifiedValue);
        }

        public void Revert(EntityStatistic entityStatistic)
        {
            float modifiedValue = valueModifier.Apply(originalValue, modifierValue);

            float distinction = modifiedValue - originalValue;

            entityStatistic.AttackDamage.Subtract(distinction);
        }
    }
}
