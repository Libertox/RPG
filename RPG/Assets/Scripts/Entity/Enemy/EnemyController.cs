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
        [SerializeField] private Transform attackCollisionPoint;

        private NavMeshAgent _agent;
        private bool isPatroling;
        private bool isFollowing;
        private bool isTakingDamage;

        private EnemyData EnemyData => (EnemyData) entityData;
        private PlayerController playerController;
        public PatrolArea PatrolArea => patrolArea;
        public bool IsTakingDamage => isTakingDamage;
        public bool IsPatroling => isPatroling;
        public bool IsFollowing => isFollowing;


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

            _agent.speed = EnemyData.MovementSpeed;
            _agent.angularSpeed = EnemyData.RotationSpeed;
            _agent.acceleration = EnemyData.Acceleration;
            isPatroling = true;

            RegisterController(new MeleeCombatController(attackCollisionPoint, EnemyData.CombatData));
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

            isFollowing = distance < EnemyData.PlayerDetectionRadius;
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

            Statistic.Health.Subtract(damage);

            if (Statistic.Health.Value <= 0)
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
