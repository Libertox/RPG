using InputSystem;
using Player.Data;
using StateMachines;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform playerPresentation;

        [Header("Settings")]
        [SerializeField] private PlayerMovementData movementData;
        public Vector2 Position => transform.position;
        public Quaternion Rotation => playerPresentation.rotation;


        private IState _idleState;
        private IState _locomotionState;

        private StateMachine _stateMachine;

        private InputManager _inputManager;

        private IAnimationController _animationController;
        private IMotionController _motionController;

        private bool _isMoving;


        [Inject]
        private void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;

            _inputManager.OnMoveStarted += OnMoveStarted;
            _inputManager.OnMoveEnded += OnMoveEnded;
        }

        private void OnMoveEnded()
        {
            _isMoving = false;
        }

        private void OnMoveStarted()
        {
            _isMoving = true;
        }

        private void Awake()
        {
            SetupRefernces();
            SetupStateMachine();
        }

        private void SetupRefernces()
        {
            _animationController = new PlayerAnimationController(playerPresentation.GetComponent<Animator>());
            _motionController = new PlayerMotionController(this, _inputManager, movementData, playerPresentation);
        }

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine();

            _idleState = new IdleState(_animationController);
            _locomotionState = new LocomotionState(_animationController, _motionController);

            _stateMachine.AddTransition(_idleState, _locomotionState, new FuncPredicate(() => _isMoving));
            _stateMachine.AddTransition(_locomotionState, _idleState, new FuncPredicate(() => !_isMoving));

            _stateMachine.SetState(_idleState);
        }

        private void Update()
        {
            _stateMachine.Update();
        }

        private void FixedUpdate()
        {
            _stateMachine.FixedUpdate();
        }
    }
}
