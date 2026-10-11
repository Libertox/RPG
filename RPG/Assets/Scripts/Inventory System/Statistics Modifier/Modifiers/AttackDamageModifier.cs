using Utility;
using Entity;

namespace InventorySystem.StatisticModifier
{
    public class AttackDamageModifier : StatisticModifier
    {
        public AttackDamageModifier(ModifierType modifierType,  float modifierValue) : base(modifierType, modifierValue)
        {
        
        }

        public override void Apply(EntityStatistic entityStatistic)
        {
            originalValue = entityStatistic.AttackDamage.Value;

            float modifiedValue = valueModifier.Apply(entityStatistic.AttackDamage.Value, modifierValue);

            entityStatistic.AttackDamage.Set(modifiedValue);
        }

        public override void Revert(EntityStatistic entityStatistic)
        {
            float modifiedValue = valueModifier.Apply(originalValue, modifierValue);

            float distinction = modifiedValue - originalValue;

            entityStatistic.AttackDamage.Subtract(distinction);
        }
    }
}
