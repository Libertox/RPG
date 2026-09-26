using Entity;
using Entity.Enemy;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Patrol State Config", menuName = "ScriptableObjects/State Machine/States/Patrol State Config")]
    public class PatrolStateConfig : BaseStateConfig
    {
        public override IState CreateState(EntityController entityController)
        {
            return new PatrolState(entityController);
        }
    }

}
