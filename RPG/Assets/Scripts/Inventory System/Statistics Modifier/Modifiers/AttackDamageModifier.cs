using Utility;
using Entity;

namespace InventorySystem.StatisticModifiers
{
    public class AttackDamageModifier : StatisticModifier
    {
        public AttackDamageModifier(StatisticModifierConfig statisticModifierConfig) : base(statisticModifierConfig)
        {
        
        }

        public override void Apply(EntityStatistic entityStatistic)
        {
            originalValue = entityStatistic.AttackDamage.Value;

            float modifiedValue = valueModifier.Apply(entityStatistic.AttackDamage.Value, config.ModifierValue);

            entityStatistic.AttackDamage.Set(modifiedValue);
        }

        public override void Revert(EntityStatistic entityStatistic)
        {
            float modifiedValue = valueModifier.Apply(originalValue, config.ModifierValue);

            float distinction = modifiedValue - originalValue;

            entityStatistic.AttackDamage.Subtract(distinction);
        }

        public override float GetModifiedValue(EntityStatistic entityStatistic)
        {
            return config.ModifierType switch
            {
                ModifierType.Addition => config.ModifierValue,
                ModifierType.Percentage => valueModifier.Apply(entityStatistic.EntityData.BaseAttackDamage, config.ModifierValue) - entityStatistic.EntityData.BaseAttackDamage,
                _ => 0,
            };
        }
    }
}
