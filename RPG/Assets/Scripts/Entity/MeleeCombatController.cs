using UnityEngine;

namespace Entity
{
    public class MeleeCombatController : ICombatController
    {
        private const int MAX_TARGET = 5;

        private readonly Transform weaponOrigin;
        private readonly Collider[] targets;
        private readonly EntityStatistic entity;
        private readonly LayerMask targetsLayerMask;

        public bool IsAttacking => isAttacking;

        private bool isAttacking;

        public MeleeCombatController(Transform weaponOrigin, EntityStatistic combatData, LayerMask targetsLayerMask)
        {
            this.weaponOrigin = weaponOrigin;
            targets = new Collider[MAX_TARGET];
            this.entity = combatData;
            this.targetsLayerMask = targetsLayerMask;
        }

        public void Attack()
        {
            int targetCount = Physics.OverlapSphereNonAlloc(weaponOrigin.position, entity.AttackRange.Value, targets, targetsLayerMask);

            if (targetCount == 0) return;

            for (int i = 0; i < targetCount; i++)
            {
                if (targets[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(entity.AttackDamage.Value);
                }
            }
        }

        public void SetIsAttacking(bool isAttacking)
        {
            this.isAttacking = isAttacking;
        }
    }
}
