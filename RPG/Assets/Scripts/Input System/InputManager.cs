using System;
using UnityEngine;

namespace InputSystem
{
    public class InputManager: IDisposable
    {
        private readonly InputActions inputActions;
        public InputEvents InputEvents { get; private set; }

        public InputManager()
        {
            inputActions = new InputActions();
            InputEvents = new InputEvents();
            inputActions.Enable();

        }

        public Vector2 GetMovementInput()
        {
            return inputActions.Player.Move.ReadValue<Vector2>();
        }

        public void Dispose()
        {
            inputActions.Dispose();
        }

  
    }
}