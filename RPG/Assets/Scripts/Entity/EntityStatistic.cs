using Utility;

namespace Entity
{
    public class EntityStatistic
    {
        public ObservableFloat Health { get; private set; }
        public ObservableInt Level {  get; private set; }
        public ObservableFloat AttackDamage { get; private set; }
        public ObservableFloat AttackRange { get; private set; }
        public ObservableFloat MovementSpeed { get; private set; }

        public EntityStatistic(EntityData entityData)
        {
            Health = new (entityData.BaseHealth);
            Level = new (entityData.BaseLevel);
            AttackDamage = new(entityData.BaseAttackDamage);
            AttackRange = new(entityData.BaseAttackRange);
            MovementSpeed = new(entityData.BaseMovementSpeed);
        }

    }
}
