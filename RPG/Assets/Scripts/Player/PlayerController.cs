using InputSystem;
using Player.Data;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMovementData movementData;

        [SerializeField] private PlayerAnimation playerAnimation;

        public Quaternion Rotation => playerAnimation.transform.rotation;

        private InputEvents _inputEvents;
        private float _turnSmoothVelocity;

        private NavMeshAgent _agent;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputEvents = inputManager.InputEvents;
            inputManager.InputEvents.OnMoveButtonPressed += Move;
            inputManager.InputEvents.OnAttackButtonPressed += Attack;
        }

        private void Move(Vector2 moveInput)
        {
            bool isMove = moveInput != Vector2.zero;

            playerAnimation.SetMoveAnimation(isMove);

            if (!isMove) return;

            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            _agent.Move(moveDirection * (movementData.MovementSpeed * Time.deltaTime));

            Rotate(moveDirection);
        }

        private void Rotate(Vector3 rotationDirection)
        {
             float targetAngle = Mathf.Atan2(rotationDirection.x, rotationDirection.z) * Mathf.Rad2Deg;
             float smoothedAngle = Mathf.SmoothDampAngle(
                                         playerAnimation.transform.eulerAngles.y,
                                         targetAngle,
                                         ref _turnSmoothVelocity,
                                         movementData.RotationSpeed * Time.deltaTime);

             playerAnimation.transform.rotation = Quaternion.Euler(0f, smoothedAngle, 0f);
        }

        private void Attack()
        {
            playerAnimation.SetAttackAnimation();
        }

        private void OnDestroy()
        {
            _inputEvents.OnMoveButtonPressed -= Move;
            _inputEvents.OnAttackButtonPressed -= Attack;
        }
    }
}
