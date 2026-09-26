

using Entity;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Is Entity Taking Damage Condition", menuName = "ScriptableObjects/Conditions/Is Entity Taking Damage Condition")]
    public class IsEntityTakingDamageCondition : EntityCondition
    {
        public override bool Evaluate(EntityController targetObject)
        {
            if (targetObject is not IDamageable damageable)
                return false;

            return isNegate ? !damageable.IsTakingDamage : damageable.IsTakingDamage;
        }
    }
}
