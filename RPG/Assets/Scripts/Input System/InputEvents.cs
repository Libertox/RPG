using System;
using UnityEngine;

namespace InputSystem
{
    public class InputEvents
    {
        public event Action<Vector2> OnMoveButtonPressed;
        public event Action OnAttackButtonPressed;
        public event Action OnInteractButtonPressed;

        public void InvokeOnMoveButtonPressed(Vector2 moveInput)
        {
            OnMoveButtonPressed?.Invoke(moveInput);
        }

        public void InvokeOnAttackButtonPressed()
        {
            OnAttackButtonPressed?.Invoke();
        }

        public void InvokeOnInteractButtonPressed()
        {
            OnInteractButtonPressed?.Invoke();
        }

    }
}
