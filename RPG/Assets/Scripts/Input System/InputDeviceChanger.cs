using InputSystem.Enums;
using UnityEngine;
using System;
using Zenject;

namespace InputSystem
{
    public class InputDeviceChanger : ITickable
    {
        public Action<ControllerType> OnControllerChanged;

        private ControllerType currentControllerType = ControllerType.PC;
        private ControllerType lastControllerType;

        public ControllerType CurrentControllerType => currentControllerType;

        public void Tick()
        {
            SetActiveController();
        }

        private void SetActiveController()
        {
            foreach (var device in UnityEngine.InputSystem.InputSystem.devices)
            {
                if (device.wasUpdatedThisFrame)
                {
                    if (device.displayName == "Mouse" || device.displayName == "Keyboard")
                        currentControllerType = ControllerType.PC;
                    else
                        currentControllerType = ControllerType.PlayStationGamepad;

                    if (lastControllerType != currentControllerType)
                    {
                        OnControllerChanged?.Invoke(currentControllerType);
                    }

                    lastControllerType = currentControllerType;
                }
            }
        }
    }
}
