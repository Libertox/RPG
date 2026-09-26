

namespace Entity
{
    public interface IDamageable
    {
        public bool IsTakingDamage { get; }

        public void TakeDamage(float damage);

        public void SetTakeDamge(bool isTakeDamge);

    }
}
