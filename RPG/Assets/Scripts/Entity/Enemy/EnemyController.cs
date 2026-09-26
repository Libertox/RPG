using Entity.Player;
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
        [SerializeField] private Transform attackCollisionPoint;

        private NavMeshAgent _agent;
        private bool isPatroling;
        private bool isFollowing;
        private bool isTakingDamage;

        private PlayerController playerController;
        public PatrolArea PatrolArea => patrolArea;
        public bool IsTakingDamage => isTakingDamage;
        public bool IsPatroling => isPatroling;
        public bool IsFollowing => isFollowing;

        private float health = 8;

        [Inject]
        private void Construct(PlayerController player)
        {
            playerController = player;
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
            isPatroling = true;

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
            if(playerController.IsDead)
            {
                isFollowing = false;
                return;
            }

            float distance = Vector3.Distance(playerController.transform.position, transform.position);

            isFollowing = distance < enemyData.PlayerDetectionRadius;
        }

        public void MoveTowardsTarget()
        {
            SetDestination(playerController.transform.position);  
        }

        public void RotateTowardsTarget()
        {
            transform.rotation = Quaternion.LookRotation(playerController.transform.position - transform.position, Vector3.up);
        }

        public void TakeDamage(float damage)
        {
            if (IsDead) return;

            health -= damage;

            if (health <= 0)
                SetIsDead(true);
            else
                SetTakeDamge(true);
        }

        public void SetTakeDamge(bool isTakimgDamge)
        {
            isTakingDamage = isTakimgDamge;
        }

        public void SetPatroling(bool isPatroling)
        {
            this.isPatroling = isPatroling;
        }

    }
}
