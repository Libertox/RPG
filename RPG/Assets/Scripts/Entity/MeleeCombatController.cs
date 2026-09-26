using UnityEngine;

namespace Entity
{
    public class MeleeCombatController : ICombatController
    {
        private const int MAX_TARGET = 5;

        private readonly Transform weaponOrigin;
        private readonly Collider[] targets;
        private readonly CombatData combatData;

        public bool IsAttacking => isAttacking;

        private bool isAttacking;

        public MeleeCombatController(Transform weaponOrigin, CombatData combatData)
        {
            this.weaponOrigin = weaponOrigin;
            targets = new Collider[MAX_TARGET];
            this.combatData = combatData;
        }

        public void Attack()
        {
            int targetCount = Physics.OverlapSphereNonAlloc(weaponOrigin.position, combatData.AttackRange, targets, combatData.TargetLayerMask);

            if (targetCount == 0) return;

            for (int i = 0; i < targetCount; i++)
            {
                if (targets[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(combatData.Damage);
                }
            }
        }

        public void SetIsAttacking(bool isAttacking)
        {
            this.isAttacking = isAttacking;
        }
    }
}
