

using Entity;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Attack State Config", menuName = "ScriptableObjects/State Machine/States/Attack State Config")]
    public class AttackStateConfig : BaseStateConfig
    {
        public override IState CreateState(EntityController entityController)
        {
            return new AttackState(entityController);
        }
    }
}
