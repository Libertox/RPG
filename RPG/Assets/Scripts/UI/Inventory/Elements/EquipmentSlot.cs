using Entity.Player;
using InventorySystem;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI.Inventory
{
    public class EquipmentSlot : MonoBehaviour
    {
        [Header("Refernces")]
        [SerializeField] private Image icon;
        [SerializeField] private Button button;

        [Header("Settings")]
        [SerializeField] private Sprite baseIcon;
        [SerializeField] private EquipmentSlotCategory category;

        private PlayerInventory _inventory;
        private ItemInventory _equipedItem;

        [Inject]
        private void Construct(PlayerController playerController)
        {
            _inventory = playerController.PlayerData.Inventory;

            _inventory.OnItemEquiped += OnItemEquiped;
        }

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnItemEquiped(ItemInventory item)
        {
            if(item.ItemBase.EquipmentSlot == category)
            {
                SetItemIcon(item.ItemBase.Icon);

                _equipedItem = item;
            }
        }

        private void SetItemIcon(Sprite icon)
        {
            this.icon.sprite = icon;
        }

        private void OnClick()
        {
            if (_equipedItem == null) return;

            _inventory.TryUnequipItem(_equipedItem);

            _equipedItem = null;

            SetItemIcon(baseIcon);
        }
    }
}
