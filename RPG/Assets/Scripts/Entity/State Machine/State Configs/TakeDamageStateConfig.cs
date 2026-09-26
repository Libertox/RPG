

using Entity;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Take Damage State Config", menuName = "ScriptableObjects/State Machine/States/Take Damage State Config")]

    public class TakeDamageStateConfig : BaseStateConfig
    {
        public override IState CreateState(EntityController entityController)
        {
            return new TakeDamageState(entityController); 
        }
    }
}
