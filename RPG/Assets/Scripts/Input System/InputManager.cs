using System;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace InputSystem
{
    public class InputManager: IDisposable, ITickable
    {
        private readonly InputActions inputActions;
        public InputEvents InputEvents { get; private set; }

        public InputManager()
        {
            inputActions = new InputActions();
            InputEvents = new InputEvents();
            inputActions.Enable();
            SubscribeInputAction();
        }

        private void SubscribeInputAction()
        {
            inputActions.Player.Attack.performed += OnAttackButtonPerformed;
        }

        private void OnAttackButtonPerformed(InputAction.CallbackContext action)
        {
            InputEvents.InvokeOnAttackButtonPressed();
        }

        public void Tick()
        {
            HandleMoveAction();
        }

        private void HandleMoveAction()
        {
            var moveActionInput = inputActions.Player.Move.ReadValue<Vector2>();

            InputEvents.InvokeOnMoveButtonPressed(moveActionInput);
        }


        public void Dispose()
        {
            inputActions.Dispose();
        }

       
    }
}