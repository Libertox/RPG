using Utility;

namespace Entity
{
    public class EntityStatistic
    {
        public ObservableFloat Health { get; private set; }
        public ObservableInt Level {  get; private set; }

        public EntityStatistic(EntityData entityData)
        {
            Health = new ObservableFloat(entityData.BaseHealth);
        }

    }
}
