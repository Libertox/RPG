using InputSystem;
using Player.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Player
{
    public class PlayerMotionController : IMotionController
    {
        private readonly InputManager _inputManager;
        private readonly NavMeshAgent _agent;
        private readonly PlayerMovementData _movementData;
        private readonly Transform _playerPresentation;

        private float _turnSmoothVelocity;

        public PlayerMotionController(PlayerController controller, InputManager inputManager, 
            PlayerMovementData playerMovementData, Transform playerPresentation)
        {
            _agent = controller.GetComponent<NavMeshAgent>();
            _inputManager = inputManager;
            _movementData = playerMovementData;
            _playerPresentation = playerPresentation;
        }
      
        public void Move()
        {
            bool isMove = _inputManager.MoveDirection != Vector2.zero;

            if (!isMove) return;

            Vector3 moveDirection = new Vector3(_inputManager.MoveDirection.x, 0, _inputManager.MoveDirection.y).normalized;

            _agent.Move(moveDirection * (_movementData.MovementSpeed * Time.deltaTime));

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
    }
}
