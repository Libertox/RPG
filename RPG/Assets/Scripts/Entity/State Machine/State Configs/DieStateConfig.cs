using Entity;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Die State Config", menuName = "ScriptableObjects/State Machine/States/Die State Config")]
    public class DieStateConfig : BaseStateConfig
    {
        public override IState CreateState(EntityController entityController)
        {
            return new DieState(entityController);
        }
    }
}
