using InventorySystem.StatisticModifiers;

namespace InventorySystem
{
    public interface IStatModifierProvider
    {
        public StatisticModifierConfig MainStatisticModifier { get; }
        public StatisticModifierConfig[] AdditionalStatisticModifiers { get; }
    }
}
