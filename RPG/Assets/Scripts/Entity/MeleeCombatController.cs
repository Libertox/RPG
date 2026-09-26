using Entity.Player;
using UnityEngine;

namespace Entity
{
    public class MeleeCombatController : ICombatController
    {
        private const int MAX_TARGET = 5;

        private readonly Transform _weaponOrigin;
        private readonly Collider[] _targets;
        private readonly CombatData _combatData;

        public bool IsAttacking => _isAttacking;

        private bool _isAttacking;

        public MeleeCombatController(Transform weaponOrigin, CombatData combatData)
        {
            _weaponOrigin = weaponOrigin;
            _targets = new Collider[MAX_TARGET];
            _combatData = combatData;
        }

        public void Attack()
        {
            int targetCount = Physics.OverlapSphereNonAlloc(_weaponOrigin.position, _combatData.AttackRange, _targets, _combatData.TargetLayerMask);

            if (targetCount == 0) return;

            for (int i = 0; i < targetCount; i++)
            {
                if (_targets[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_combatData.Damage);
                }
            }
        }

        public void SetIsAttacking(bool isAttacking)
        {
            _isAttacking = isAttacking;
        }
    }
}
