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

        private float _turnSmoothVelocity;

        private InputManager inputManager;

        private NavMeshAgent agent;

        [Inject]
        public void Construct(InputManager inputManager)
        {
            this.inputManager = inputManager;
        }

        private void Awake()
        {
            agent = GetComponent<NavMeshAgent>();
        }

        private void Update()
        {
            Move();
        }

        private void Move()
        {
            Vector2 moveInput = inputManager.GetMovementInput();

            bool isMove = moveInput != Vector2.zero;

            playerAnimation.SetMoveAnimation(isMove);

            if (!isMove) return;

            Vector3 moveDirection = new Vector3(moveInput.x, 0, moveInput.y).normalized;

            agent.Move(moveDirection * (movementData.MovementSpeed * Time.deltaTime));

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
    }
}
