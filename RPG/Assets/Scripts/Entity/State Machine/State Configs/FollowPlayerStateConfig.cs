using Entity;
using Entity.Enemy;
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Follow State Config", menuName = "ScriptableObjects/State Machine/States/Follow State Config")]
    public class FollowPlayerStateConfig : BaseStateConfig
    {
        public override IState CreateState(EntityController entityController)
        {
            return new FollowPlayerState(entityController);
        }
    }
}
