using InputSystem.Enums;
using UnityEngine;

namespace InputSystem
{
    [CreateAssetMenu(fileName = nameof(InputIconContainer), menuName = "ScriptableObjects/Input System/"+nameof(InputIconContainer))]
    public class InputIconContainer : ScriptableObject
    {
        [field: SerializeField] private InputIcon inputIcon;

        public Sprite GetInputIcons(ControllerType controllerType)
        {
            return inputIcon.GetIcon(controllerType);
        }

    }

    [System.Serializable]
    public struct InputIcon
    {
        public Sprite PcIcon;
        public Sprite PlayStationIcon;
        public Sprite XboxIcon;

        public readonly Sprite GetIcon(ControllerType controllerType)
        {
            return controllerType switch
            {
                ControllerType.PC => PcIcon,
                ControllerType.PlayStationGamepad => PlayStationIcon,
                ControllerType.XboxGamepad => XboxIcon,
                _ => null,
            };
        }
    }
}
