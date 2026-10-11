

using Entity;
using Mono.Cecil;
using Utility;

namespace InventorySystem.StatisticModifier
{
    public abstract class StatisticModifier
    {
        protected readonly IValueModifier valueModifier;
        protected readonly float modifierValue;
        public ModifierType Type { get; private set; }

        protected float originalValue;

        public StatisticModifier(ModifierType modifierType, float modifierValue)
        {
            this.valueModifier = ValueModifierFactory.Create(modifierType);
            this.modifierValue = modifierValue;
            Type = modifierType;
        }

        public abstract void Apply(EntityStatistic entityStatistic);
        public abstract void Revert(EntityStatistic entityStatistic);
    }
}
