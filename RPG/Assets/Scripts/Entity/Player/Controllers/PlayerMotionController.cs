using InputSystem;
using System;
using UnityEngine;
using UnityEngine.AI;

namespace Entity.Player
{
    public class PlayerMotionController : IMotionController, IDisposable
    {
        private readonly InputManager _inputManager;
        private readonly NavMeshAgent _agent;
        private readonly PlayerData _movementData;
        private readonly Transform _playerPresentation;
        private readonly PlayerController _playerController;

        private float _turnSmoothVelocity;
        private bool _isMoving;

        public bool IsMoving => _isMoving;

        public PlayerMotionController(PlayerController controller, InputManager inputManager,
            PlayerData playerMovementData, Transform playerPresentation)
        {
            _agent = controller.GetComponent<NavMeshAgent>();
            _inputManager = inputManager;
            _movementData = playerMovementData;
            _playerPresentation = playerPresentation;
            _playerController = controller;

            _inputManager.OnMoveStarted += OnMoveStarted;
            _inputManager.OnMoveCanceled += OnMoveCanceled;
        }

        private void OnMoveCanceled()
        {
            _isMoving = false;
        }

        private void OnMoveStarted()
        {
            _isMoving = true;
        }

        public void Move()
        {
            bool isMove = _inputManager.MoveDirection != Vector2.zero;

            if (!isMove) return;

            Vector3 moveDirection = new Vector3(_inputManager.MoveDirection.x, 0, _inputManager.MoveDirection.y).normalized;

            _agent.Move(moveDirection * (_playerController.GetMovementSpeed() * Time.deltaTime));

            Rotate(moveDirection);
        }

        public void Rotate(Vector3 rotationDirection)
        {
             float targetAngle = Mathf.Atan2(rotationDirection.x, rotationDirection.z) * Mathf.Rad2Deg;
             float smoothedAngle = Mathf.SmoothDampAngle(
                                         _playerPresentation.transform.eulerAngles.y,
                                         targetAngle,
                                         ref _turnSmoothVelocity,
                                         _movementData.RotationSpeed * Time.deltaTime);

            _playerPresentation.transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }

        public void Dispose()
        {
            _inputManager.OnMoveStarted -= OnMoveStarted;
            _inputManager.OnMoveCanceled -= OnMoveCanceled;
        }
    }
}
