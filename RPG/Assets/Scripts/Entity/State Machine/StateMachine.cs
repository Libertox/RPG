using Entity;
using System.Collections.Generic;
using UnityEngine;
using Utility;

namespace StateMachines
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private BaseStateConfig defaultState;
        [SerializeField] private Transition[] transitions;
        [SerializeField] private Transition[] anyTransitions;

        [SerializeField] private EntityController entityController;

        private StateNode current;

        private readonly Dictionary<BaseStateConfig, StateNode> nodes = new();

        private void Start()
        {
            foreach (var transition in transitions)
            {
                AddTransition(transition);
            }

            foreach (var transition in anyTransitions)
            {
                AddAnyTransition(transition);
            }

            SetState(defaultState);
        }

        private void Update()
        {
            var transition = GetTransition();

            if (transition != null)
                ChangeState(transition.To);

            current.State?.Update();

            Debug.Log(current.State?.ToString());
        }

        private void FixedUpdate()
        {
            current.State?.FixedUpdate();
        }

        public void SetState(BaseStateConfig state)
        {
            current = nodes[state];
            current.State?.OnEnter();
        }

        private void ChangeState(BaseStateConfig state)
        {
            if (state == current.StateConfig) return;

            var previousState = current.State;
            var nextState = nodes[state].State;

            previousState?.OnExit();
            nextState?.OnEnter();
            current = nodes[state];
        }

        private ITransition GetTransition()
        {
            foreach (var transition in anyTransitions)
                if (transition.Conditions.Evaluate(entityController))
                    return transition;

            foreach (var transition in current.Transitions)
                if (transition.Conditions.Evaluate(entityController))
                    return transition;

            return null;
        }

        public void AddTransition(Transition transition)
        {
            GetOrAddNode(transition.From).AddTransition(transition);
        }

        public void AddAnyTransition(Transition transition)
        {
            GetOrAddNode(transition.To);
        }

        private StateNode GetOrAddNode(BaseStateConfig state)
        {
            var node = nodes.GetValueOrDefault(state);

            if (node == null)
            {
                node = new StateNode(state, entityController);
                nodes.Add(state, node);
            }

            return node;
        }

        private class StateNode
        {
            public BaseStateConfig StateConfig { get; }
            public IState State { get; }
            public HashSet<Transition> Transitions { get; }

            public StateNode(BaseStateConfig stateConfig, EntityController entityController)
            {
                StateConfig = stateConfig;
                State = stateConfig.CreateState(entityController);
                Transitions = new HashSet<Transition>();
            }

            public void AddTransition(Transition transition)
            {
                Transitions.Add(transition);
            }
        }

    }
}
