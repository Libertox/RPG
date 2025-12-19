

namespace Entity
{
    public interface ICombatController
    {
        public bool IsAttacking { get; }

        public void Attack();
        public void SetIsAttacking(bool isAttacking);

    }
}
