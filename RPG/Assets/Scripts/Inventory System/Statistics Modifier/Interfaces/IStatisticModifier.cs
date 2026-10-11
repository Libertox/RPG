using Entity;

namespace InventorySystem.StatisticModifier
{
    public interface IStatisticModifier
    {
        public void Apply(EntityStatistic entityStatistic);
        public void Revert(EntityStatistic entityStatistic);
    }
}
