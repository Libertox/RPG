using Entity;
using Entity.Enemy;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Waiting State Config", menuName = "ScriptableObjects/State Machine/States/Waiting State Config")]
    public class WaitingStateConfig : BaseStateConfig
    {
        [SerializeField] private float waitTime;

        public override IState CreateState(EntityController entityController)
        {
            return new WaitingState(entityController, waitTime);
        }
    }
}
