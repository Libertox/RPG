
using Entity;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Is Entity Moving Condition", menuName = "ScriptableObjects/Conditions/Is Entity Moving Condition")]
    public class IsEntityMovingCondition : EntityCondition
    {
        public override bool Evaluate(EntityController targetObject)
        {
            IMotionController motionController = targetObject.GetController<IMotionController>();

            return isNegate ? !motionController.IsMoving : motionController.IsMoving;
        }
    }
}
