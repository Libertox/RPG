
using UnityEngine;

namespace StateMachines
{
    [CreateAssetMenu(fileName = "Transition", menuName = "ScriptableObjects/State Machine/Transitions/Transition")]
    public class Transition : ScriptableObject, ITransition
    {
        [SerializeField] private EntityCondition[] conditions;
        [SerializeField] private BaseStateConfig fromState;
        [SerializeField] private BaseStateConfig toState;


        public EntityCondition[] Conditions => conditions;
        public BaseStateConfig To => toState;

        public BaseStateConfig From => fromState;
    }
}
