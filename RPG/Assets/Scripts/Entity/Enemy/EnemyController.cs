using Entity.Player;
using StateMachines;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Area;


namespace Entity.Enemy
{
    public class EnemyController : EntityController, IDamageable
    {
        [SerializeField] private PatrolArea patrolArea;

        [SerializeField] private EnemyData enemyData;

        [SerializeField] private Animator animator;

        [SerializeField] private Transform attackCollisionPoint;

        private IState _patrolState;
        private IState _idleState;
        private IState _followState;
        private IState _attackState;
        private IState _takeDamageState;
        private IState _dieState;

        private NavMeshAgent _agent;
        private StateMachine _stateMachine;

        private bool _isPatroling;
        private bool _isFollowing;
        private bool _isTakingDamage;
        private bool _isDead;

        private PlayerController _playerController;

        private float _health = 8;

        [Inject]
        private void Construct(PlayerController player)
        {
            _playerController = player;
        }

        private void Awake()
        {
            InitializeAgent();
            SetupStateMachine();
        }

        private void InitializeAgent()
        {
            _agent = GetComponent<NavMeshAgent>();

            _agent.speed = enemyData.MovementSpeed;
            _agent.angularSpeed = enemyData.RotationSpeed;
            _agent.acceleration = enemyData.Acceleration;

            _animationController = new EntityAnimatorController(animator);
            _combatController = new MeleeCombatController(attackCollisionPoint, enemyData.CombatData);
        }

        private void SetupStateMachine() 
        {
            _isPatroling = true;

            _stateMachine = new StateMachine();

            _patrolState = new PatrolState(this, patrolArea);
            _idleState = new WaitingState(this, enemyData.WaitingTime);
            _followState = new FollowState(this);
            _attackState = new AttackState(this);
            _takeDamageState = new TakeDamageState(this);
            _dieState = new DieState(this);

            _stateMachine.AddTransition(_idleState, _patrolState, new FuncPredicate(() => _isPatroling));
            _stateMachine.AddTransition(_patrolState, _idleState, new FuncPredicate(() => !_isPatroling));

            _stateMachine.AddTransition(_idleState, _followState, new FuncPredicate(() => _isFollowing));
            _stateMachine.AddTransition(_patrolState, _followState, new FuncPredicate(() => _isFollowing));

            _stateMachine.AddTransition(_followState, _patrolState, new FuncPredicate(() => !_isFollowing));

            _stateMachine.AddTransition(_followState, _attackState, new FuncPredicate(() => _combatController.IsAttacking));
            _stateMachine.AddTransition(_attackState, _followState, new FuncPredicate(() => !_combatController.IsAttacking));

            _stateMachine.AddTransition(_takeDamageState, _followState, new FuncPredicate(() => !_isTakingDamage));

            _stateMachine.AddAnyTransition(_takeDamageState, new FuncPredicate(() => _isTakingDamage));
            _stateMachine.AddAnyTransition(_dieState, new FuncPredicate(() => _isDead));

            _stateMachine.SetState(_patrolState);
        }

        private void Update()
        {
            _stateMachine.Update();
            FindFollowTarget();
        }

        public void SetDestination(Vector3 destinationPosition)
        {
            _agent.SetDestination(destinationPosition);
        }

        public bool IsOnDestination()
        {
            return _agent.remainingDistance < 1.1f;
        }

        public void FindFollowTarget()
        {
            if(_playerController.IsDead)
            {
                _isFollowing = false;
                return;
            }

            float distance = Vector3.Distance(_playerController.transform.position, transform.position);

            _isFollowing = distance < enemyData.PlayerDetectionRadius;
        }

        public void MoveTowardsTarget()
        {
            SetDestination(_playerController.transform.position);  
        }

        public void RotateTowardsTarget()
        {
            transform.rotation = Quaternion.LookRotation(_playerController.transform.position - transform.position, Vector3.up);
        }

        public void TakeDamage(float damage)
        {
            if (_isDead) return;

            _health -= damage;

            if (_health <= 0)
                SetIsDead(true);
            else
                SetTakeDamge(true);
        }

        public void SetTakeDamge(bool isTakimgDamge)
        {
            _isTakingDamage = isTakimgDamge;
        }

        public void SetIsDead(bool isDead)
        {
            _isDead = isDead;
        }

        public void SetPatroling(bool isPatroling)
        {
            _isPatroling = isPatroling;
        }

        public override void Destroy()
        {

        }



    }
}
