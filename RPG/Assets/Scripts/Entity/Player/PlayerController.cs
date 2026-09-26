using InputSystem;
using InventorySystem;
using System;
using UnityEngine;
using Zenject;

namespace Entity.Player
{
    public class PlayerController : EntityController, IDamageable
    {
        public event Action OnDie;

        [SerializeField] private Transform playerPresentation;
        [SerializeField] private Transform attackCollisionPoint;

        private InputManager inputManager;
        private bool isTakingDamage;

        public Vector3 Position => transform.localPosition;
        public Quaternion Rotation => playerPresentation.rotation;
        public bool IsTakingDamage => isTakingDamage;
        public PlayerInventory PlayerInventory { get; private set; }
        public PlayerData PlayerData => (PlayerData)entityData;



        [Inject]
        private void Construct(InputManager inputManager, PlayerInventory playerInventory)
        {
            PlayerInventory = playerInventory;
            this.inputManager = inputManager;

            this.inputManager.OnAttackPressed += OnAttackButtonPressed;
        }

        private void OnAttackButtonPressed()
        {
            if (GetController<ICombatController>().IsAttacking || GetController<IAnimationController>().IsWaitingForEndAnimation) return;

            GetController<ICombatController>().SetIsAttacking(true);
        }

        protected override void Awake()
        {
            base.Awake();

            SetupRefernces();
        }

        private void SetupRefernces()
        {
            RegisterController(new PlayerMotionController(this, inputManager, PlayerData, playerPresentation));
            RegisterController(new MeleeCombatController(attackCollisionPoint, PlayerData.CombatData));
        }


        public void TakeDamage(float damage)
        {
            if (IsDead) return;

            Statistic.Health.Subtract(damage);

            if (Statistic.Health.Value <= 0) Die();
            else SetTakeDamge(true);
        }

        private void Die()
        {
            SetIsDead(true);
            OnDie?.Invoke();
        }

        public void SetTakeDamge(bool isTakimgDamge)
        {
            isTakingDamage = isTakimgDamge;
        }

        public float GetMovementSpeed()
        {
            float speed = PlayerInventory.Weight >= PlayerData.MaxLiftingCapacity 
                ? PlayerData.EncumberedSpeed : PlayerData.MovementSpeed;

            return speed;
        }
    }
}
