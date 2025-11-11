using InputSystem;
using Player.Data;
using StateMachines;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Transform playerPresentation;

        [SerializeField] private Transform attackCollisionPoint;

        [Header("Settings")]
        [SerializeField] private PlayerMovementData movementData;
        [SerializeField] private PlayerCombatData combatData;

        public Vector3 Position => transform.localPosition;
        public Quaternion Rotation => playerPresentation.rotation;

        private IState _idleState;
        private IState _locomotionState;
        private IState _attackState;

        private StateMachine _stateMachine;

        private InputManager _inputManager;

        private IAnimationController _animationController;
        private IMotionController _motionController;
        private ICombatController _combatController;

        private bool _isMoving;
        public bool _isAttacking;


        [Inject]
        private void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;

            _inputManager.OnMoveStarted += OnMoveStarted;
            _inputManager.OnMoveEnded += OnMoveEnded;
            _inputManager.OnAttackButtonPressed += OnAttackButtonPressed;
        }

        private void OnAttackButtonPressed()
        {
            _isAttacking = true;
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
            _combatController = new PlayerCombatController(attackCollisionPoint, combatData);
        }

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine();

            _idleState = new IdleState(_animationController);
            _locomotionState = new LocomotionState(_animationController, _motionController);
            _attackState = new AttackState(_animationController, this, _combatController);

            _stateMachine.AddTransition(_idleState, _locomotionState, new FuncPredicate(() => _isMoving));
            _stateMachine.AddTransition(_locomotionState, _idleState, new FuncPredicate(() => !_isMoving));

            _stateMachine.AddTransition(_idleState, _attackState, new FuncPredicate(() => _isAttacking));
            _stateMachine.AddTransition(_locomotionState, _attackState, new FuncPredicate(() => _isAttacking));

            _stateMachine.AddTransition(_attackState, _idleState, new FuncPredicate(() => !_isAttacking));

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
