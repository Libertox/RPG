

using Entity;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Is Entity Dead Condition", menuName = "ScriptableObjects/Conditions/Is Entity Dead Condition")]
    public class IsEntityDeadCondition : EntityCondition
    {
        public override bool Evaluate(EntityController targetObject)
        {
            return isNegate ? !targetObject.IsDead : targetObject.IsDead;
        }
    }
}
