

using Entity;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Is Entity Patroling Condition", menuName = "ScriptableObjects/Conditions/Is Entity Patroling Condition")]
    public class IsEntityPatrolingCondition : EntityCondition
    {
        public override bool Evaluate(EntityController targetObject)
        {
            if (targetObject is not IPatrolable patrolable)
                return false;

            return isNegate ? !patrolable.IsPatroling : patrolable.IsPatroling;
        }
    }
}
