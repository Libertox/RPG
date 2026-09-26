
using Entity;
using UnityEngine;

namespace StateMachines
{
    public abstract class BaseStateConfig : ScriptableObject
    {
        public abstract IState CreateState(EntityController entityController);
    }
 
}
