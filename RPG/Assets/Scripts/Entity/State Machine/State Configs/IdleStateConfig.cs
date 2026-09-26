

using Entity;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Idle State Config", menuName = "ScriptableObjects/State Machine/States/Idle State Config")]
    public class IdleStateConfig : BaseStateConfig
    {
        public override IState CreateState(EntityController entityController)
        {
            return new IdleState(entityController);
        }
    }
}
