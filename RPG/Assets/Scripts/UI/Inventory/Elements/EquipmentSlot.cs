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
        [SerializeField] private Sprite defaultIcon;
        [SerializeField] private EquipmentSlotCategory slotCategory;
        [SerializeField] private int slotIndex;

        private Equipment _equipment;

        [Inject]
        private void Construct(PlayerController playerController)
        {
            _equipment = playerController.PlayerInventory.Equipment;
        }

        private void Awake()
        {
            button.onClick.AddListener(OnClick);
        }

        private void OnEnable()
        {
            button.onClick.AddListener(OnClick);
            _equipment.OnItemEquipped += HandleItemEquipped;
            Refresh();
        }

        private void OnDisable()
        {
            button.onClick.RemoveListener(OnClick);
            _equipment.OnItemEquipped -= HandleItemEquipped;
        }

        private void HandleItemEquipped(ItemInventory item, int index)
        {
            if (Matches(item, index))
            {
                SetIcon(item.ItemBase.Icon);
            }
        }

        private bool Matches(ItemInventory item, int index)
        {
            return item.ItemBase.EquipmentSlot == slotCategory && index == slotIndex;
        }

        private void Refresh()
        {
            var item = _equipment.GetEquipped(slotCategory, slotIndex);
            SetIcon(item != null ? item.ItemBase.Icon : defaultIcon);
        }

        private void SetIcon(Sprite icon)
        {
            this.icon.sprite = icon;
        }

        private void OnClick()
        {
            if (_equipment.TryUnequipItem(slotCategory, slotIndex))
                SetIcon(defaultIcon);
        }
    }
}
