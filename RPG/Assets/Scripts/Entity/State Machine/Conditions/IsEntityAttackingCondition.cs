
using Entity;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Is Entity Attacking Condition", menuName = "ScriptableObjects/Conditions/Is Entity Attacking Condition")]
    public class IsEntityAttackingCondition : EntityCondition
    {
        public override bool Evaluate(EntityController targetObject)
        {
            ICombatController controller = targetObject.GetController<ICombatController>();

            return isNegate ? !controller.IsAttacking : controller.IsAttacking;

        }
    }
}
