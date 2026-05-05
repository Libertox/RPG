using Entity.Player;
using InputSystem;
using InventorySystem;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.Inventory
{
    public class EquipmentSlot : MonoBehaviour, IDragable
    {
        [Header("Refernces")]
        [SerializeField] private Image icon;
        [SerializeField] private Button button;
        [SerializeField] private ItemHolder holder;

        [Header("Settings")]
        [SerializeField] private EquipmentSlotCategory slotCategory;
        [SerializeField] private int slotIndex;

        private Equipment _equipment;
        private InputManager _inputManager;
        private IInventoryStorage _inventoryStorage;

        [Inject]
        private void Construct(PlayerController playerController, InputManager inputManager) 
        {
            _equipment = playerController.PlayerInventory.Equipment;
            _inventoryStorage = playerController.PlayerInventory.InventoryStorage;
            _inputManager = inputManager;
        }

        private void Awake()
        {
            button.onClick.AddListener(OnClick);

            SetIcon(slotCategory.Icon);
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
            SetIcon(item != null ? item.ItemBase.Icon : slotCategory.Icon);
        }

        private void SetIcon(Sprite icon)
        {
            this.icon.sprite = icon;
        }

        private void OnClick()
        {
            var item = _equipment.GetEquipped(slotCategory, slotIndex);

            if (_equipment.TryUnequipItem(slotCategory, slotIndex))
            {
                SetIcon(slotCategory.Icon);
                _inventoryStorage.AddItemAndNotify(item.ItemBase);
            }
                
        }

        public void Drag()
        {
            holder.SetItem(_equipment.GetEquipped(slotCategory, slotIndex), this);

            if (_equipment.TryUnequipItem(slotCategory, slotIndex))
                SetIcon(slotCategory.Icon);
        }

        public void Drop()
        {
            if (holder.HoldItem == null) return;

            if (slotCategory == holder.HoldItem.ItemBase.EquipmentSlot)
                _equipment.TryEquipItem(holder.HoldItem, slotIndex);
            else
                holder.ReturnToStartSlot();

           holder.Hide();
        }
    }
}
