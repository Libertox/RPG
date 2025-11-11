using InputSystem;
using UnityEngine;
using Zenject;

namespace Player
{
    public class PlayerCombatController : MonoBehaviour, ICombatController
    {
        [SerializeField] private PlayerAnimationController playerAnimation;

        private InputManager _inputManager;

        [Inject]
        public void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
            //inputManager.OnAttackButtonPressed += Attack;
        }

        public void Attack()
        {
            playerAnimation.SetAttackAnimation();
        }


        private void OnDestroy()
        {
            _inputManager.OnAttackButtonPressed -= Attack;
        }

    }
}
