using InventorySystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

namespace Utility
{
    public class DebugManager : MonoBehaviour
    {
        [Header("Input Actions")]
        [SerializeField] private InputAction showInventory;

        private PlayerInventory playerInventory;

        [Inject]
        private void Construct(PlayerInventory playerInventory)
        {
            this.playerInventory = playerInventory;
        }

        private void Start()
        {
            showInventory.Enable();
            showInventory.performed += OnShowInventory;
        }

        private void OnShowInventory(InputAction.CallbackContext context)
        {
            playerInventory.InventoryStorage.Show();
        }
    }
}
