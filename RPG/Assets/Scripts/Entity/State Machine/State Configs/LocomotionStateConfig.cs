using Entity;
using Entity.Player;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Locomotion State Config", menuName = "ScriptableObjects/State Machine/States/Locomotion State Config")]
    public class LocomotionStateConfig : BaseStateConfig
    {
        public override IState CreateState(EntityController entityController)
        {
            return new LocomotionState(entityController);
        }
    }
}
