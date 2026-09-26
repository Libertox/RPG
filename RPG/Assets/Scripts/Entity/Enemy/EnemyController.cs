using Entity.Player;
using StateMachines;
using UnityEngine;
using UnityEngine.AI;
using Zenject;
using Area;


namespace Entity.Enemy
{
    public class EnemyController : EntityController, IDamageable, IPatrolable, IPlayerFollower
    {
        [SerializeField] private PatrolArea patrolArea;

        [SerializeField] private EnemyData enemyData;

        [SerializeField] private Animator animator;

        [SerializeField] private Transform attackCollisionPoint;

        private NavMeshAgent _agent;
        private bool _isPatroling;
        private bool _isFollowing;
        private bool _isTakingDamage;

        private PlayerController _playerController;
        public PatrolArea PatrolArea => patrolArea;
        public bool IsTakingDamage => _isTakingDamage;
        public bool IsPatroling => _isPatroling;
        public bool IsFollowing => _isFollowing;

        private float _health = 8;

        [Inject]
        private void Construct(PlayerController player)
        {
            _playerController = player;
        }

        protected override void Awake()
        {
            base.Awake();

            InitializeAgent();
        }

        private void InitializeAgent()
        {
            _agent = GetComponent<NavMeshAgent>();

            _agent.speed = enemyData.MovementSpeed;
            _agent.angularSpeed = enemyData.RotationSpeed;
            _agent.acceleration = enemyData.Acceleration;
            _isPatroling = true;

            RegisterController(new MeleeCombatController(attackCollisionPoint, enemyData.CombatData));
        }

        private void Update()
        {
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
            if (IsDead) return;

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

        public void SetPatroling(bool isPatroling)
        {
            _isPatroling = isPatroling;
        }

    }
}
