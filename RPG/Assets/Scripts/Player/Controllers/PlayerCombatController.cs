
using Player.Data;
using UnityEngine;

namespace Player
{
    public class PlayerCombatController : ICombatController
    {
        private const int MAX_TARGET = 5;

        private readonly Transform _weaponOrigin;
        private readonly Collider[] _targets;
        private readonly PlayerCombatData _combatData;

        public PlayerCombatController(Transform weaponOrigin, PlayerCombatData combatData)
        {
            _weaponOrigin = weaponOrigin;
            _targets = new Collider[MAX_TARGET];
            _combatData = combatData;
        }

        public void Attack()
        {
            int targetCount = Physics.OverlapSphereNonAlloc(_weaponOrigin.position, _combatData.AttackRange, _targets, _combatData.TargetLayerMask);

            if (targetCount == 0) return;

            for(int i = 0; i < targetCount; i++)
            {
                if (_targets[i].TryGetComponent(out IDamageable damageable))
                {
                    damageable.TakeDamage(_combatData.Damage);
                }
            }
        }

    }
}
