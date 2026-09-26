using InputSystem;
using InventorySystem;
using StateMachines;
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

        [Header("Settings")]
        [SerializeField] private PlayerData playerData;

        public Vector3 Position => transform.localPosition;
        public Quaternion Rotation => playerPresentation.rotation;

        public PlayerInventory PlayerInventory { get; private set; }
        public PlayerData PlayerData => playerData;

        private InputManager _inputManager;

        private bool _isTakingDamage;
        private float _health = 10;

        [field:SerializeField] public int Level { get; private set; }

        public bool IsTakingDamage => _isTakingDamage;

        [Inject]
        private void Construct(InputManager inputManager, PlayerInventory playerInventory)
        {
            PlayerInventory = playerInventory;
            _inputManager = inputManager;

            _inputManager.OnAttackPressed += OnAttackButtonPressed;
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
            RegisterController(new PlayerMotionController(this, _inputManager, playerData, playerPresentation));
            RegisterController(new MeleeCombatController(attackCollisionPoint, playerData.CombatData));
        }


        public void TakeDamage(float damage)
        {
            if (IsDead) return;

            _health -= damage;

            if (_health <= 0) Die();
            else SetTakeDamge(true);
        }

        private void Die()
        {
            SetIsDead(true);
            OnDie?.Invoke();
        }

        public void SetTakeDamge(bool isTakimgDamge)
        {
            _isTakingDamage = isTakimgDamge;
        }

        public float GetMovementSpeed()
        {
            float speed = PlayerInventory.Weight >= playerData.MaxLiftingCapacity 
                ? playerData.EncumberedSpeed : playerData.MovementSpeed;

            return speed;
        }
    }
}
