using Entity.Player;
using InputSystem;
using TMPro;
using UI;
using UnityEngine;
using Zenject;

namespace InventorySystem.UI
{
    public class LiftingVisualizer : UIVisualizer
    {
        [SerializeField] private TextMeshProUGUI liftingCapacityValue;

        private PlayerController playerController;

        [Inject]
        public void Construct(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        public override void Refresh()
        {
            UpdateLiftingCapacityValue(playerController.PlayerInventory.Weight, playerController.PlayerData.MaxLiftingCapacity);
        }

        private void UpdateLiftingCapacityValue(float value, float maxValue)
        {
            liftingCapacityValue.SetText($"{value}/{maxValue}");
        }
    }
}
