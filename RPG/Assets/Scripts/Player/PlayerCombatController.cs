using InputSystem;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerCombatController : MonoBehaviour, ICombatController
    {
        [SerializeField] private PlayerAnimation playerAnimation;

        private InputEvents _inputEvents;

        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputEvents = inputManager.InputEvents;
            inputManager.InputEvents.OnAttackButtonPressed += Attack;
        }

        public void Attack()
        {
            playerAnimation.SetAttackAnimation();
        }


        private void OnDestroy()
        {
            _inputEvents.OnAttackButtonPressed -= Attack;
        }

    }
}
