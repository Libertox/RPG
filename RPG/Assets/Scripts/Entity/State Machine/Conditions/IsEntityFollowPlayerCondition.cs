using Entity;
using StateMachines;
using UnityEngine;

namespace Assets.Scripts.Entity.State_Machine.Conditions
{
    [CreateAssetMenu(fileName = "Is Entity Follow Player Condition", menuName = "ScriptableObjects/Conditions/Is Entity Follow Player Condition")]
    public class IsEntityFollowPlayerCondition : EntityCondition
    {
        public override bool Evaluate(EntityController targetObject)
        {
            if (targetObject is not IPlayerFollower playerFollower)
                return false;

            return isNegate ? !playerFollower.IsFollowing : playerFollower.IsFollowing;
        }
    }
}
