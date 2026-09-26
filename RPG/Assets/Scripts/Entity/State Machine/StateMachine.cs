using Entity;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachines
{
    public class StateMachine : MonoBehaviour
    {
        [SerializeField] private BaseStateConfig defaultState;
        [SerializeField] private Transition[] transitions;
        [SerializeField] private Transition[] anyTransitions;

        [SerializeField] private EntityController entityController;

        private StateNode _current;

        private readonly Dictionary<BaseStateConfig, StateNode> _nodes = new();
        private readonly HashSet<ITransition> _anyTransitions = new();

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

            _current.State?.Update();

            Debug.Log(_current.State?.ToString());
        }

        private void FixedUpdate()
        {
            _current.State?.FixedUpdate();
        }

        public void SetState(BaseStateConfig state)
        {
            _current = _nodes[state];
            _current.State?.OnEnter();
        }

        private void ChangeState(BaseStateConfig state)
        {
            if (state == _current.StateConfig) return;

            var previousState = _current.State;
            var nextState = _nodes[state].State;

            previousState?.OnExit();
            nextState?.OnEnter();
            _current = _nodes[state];
        }

        private ITransition GetTransition()
        {
            foreach (var transition in _anyTransitions)
                if (transition.Conditions.Evaluate(entityController))
                    return transition;

            foreach (var transition in _current.Transitions)
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
            _anyTransitions.Add(transition);
            GetOrAddNode(transition.To);
        }

        private StateNode GetOrAddNode(BaseStateConfig state)
        {
            var node = _nodes.GetValueOrDefault(state);

            if (node == null)
            {
                node = new StateNode(state, entityController);
                _nodes.Add(state, node);
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
