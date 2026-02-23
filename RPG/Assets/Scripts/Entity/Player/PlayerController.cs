using InputSystem;
using StateMachines;
using System;
using UI;
using UnityEngine;
using Zenject;

namespace Entity.Player
{
    public class PlayerController : EntityController, IDamageable
    {
        public event Action OnDie;

        [SerializeField] private Transform playerPresentation;

        [SerializeField] private Transform attackCollisionPoint;

        [Header("Settings")]
        [SerializeField] private PlayerData playerData;

        public Vector3 Position => transform.localPosition;
        public Quaternion Rotation => playerPresentation.rotation;

        public PlayerData PlayerData => playerData;

        private IState _idleState;
        private IState _locomotionState;
        private IState _attackState;
        private IState _takeDamageState;
        private IState _dieState;

        private StateMachine _stateMachine;

        private InputManager _inputManager;
        private UIViewManager _viewManager;

        private IMotionController _motionController;

        private bool _isMoving;
        private bool _isTakingDamage;
        private bool _isDead;

        private float _health = 10;

        public bool IsDead => _isDead;

        [Inject]
        private void Construct(InputManager inputManager, UIViewManager viewManager)
        {
            _inputManager = inputManager;
            _viewManager = viewManager;

            _inputManager.OnMoveStarted += OnMoveStarted;
            _inputManager.OnMoveEnded += OnMoveEnded;
            _inputManager.OnAttackButtonPressed += OnAttackButtonPressed;
        }

        private void OnAttackButtonPressed()
        {
            if (_combatController.IsAttacking || _animationController.IsWaitingForEndAnimation) return;

            _combatController.SetIsAttacking(true);
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
            _motionController = new PlayerMotionController(this, _inputManager, playerData, playerPresentation);
            _combatController = new MeleeCombatController(attackCollisionPoint, playerData.CombatData);
        }

        private void SetupStateMachine()
        {
            _stateMachine = new StateMachine();

            _idleState = new IdleState(this);
            _locomotionState = new LocomotionState(this, _motionController);
            _attackState = new AttackState(this);
            _takeDamageState = new TakeDamageState(this);
            _dieState = new DieState(this);

            _stateMachine.AddTransition(_idleState, _locomotionState, new FuncPredicate(() => _isMoving));
            _stateMachine.AddTransition(_locomotionState, _idleState, new FuncPredicate(() => !_isMoving));

            _stateMachine.AddTransition(_idleState, _attackState, new FuncPredicate(() => _combatController.IsAttacking));
            _stateMachine.AddTransition(_locomotionState, _attackState, new FuncPredicate(() => _combatController.IsAttacking));

            _stateMachine.AddTransition(_attackState, _idleState, new FuncPredicate(() => !_combatController.IsAttacking));

            _stateMachine.AddTransition(_takeDamageState, _idleState, new FuncPredicate(() => !_isMoving));
            _stateMachine.AddTransition(_takeDamageState, _locomotionState, new FuncPredicate(() => _isMoving));
            _stateMachine.AddTransition(_takeDamageState, _attackState, new FuncPredicate(() => _combatController.IsAttacking));

            _stateMachine.AddAnyTransition(_takeDamageState, new FuncPredicate(() => _isTakingDamage));
            _stateMachine.AddAnyTransition(_dieState, new FuncPredicate(() => _isDead));

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

        public void TakeDamage(float damage)
        {
            if (_isDead) return;

            _health -= damage;

            if (_health <= 0) Die();
            else SetTakeDamge(true);
        }

        private async void Die()
        {
            SetIsDead(true);
            OnDie?.Invoke();
            await _viewManager.TryOpenView<GameOverView>();
        }

        public void SetTakeDamge(bool isTakimgDamge)
        {
            _isTakingDamage = isTakimgDamge;
        }

        public void SetIsDead(bool isDead)
        {
            _isDead = isDead;
        }
    }
}
