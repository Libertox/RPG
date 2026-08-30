using Entity.Player;
using InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem.UI
{
    public class EquipmentSlot : MonoBehaviour, IItemContainer
    {
        [Header("Refernces")]
        [SerializeField] private Image icon;
        [SerializeField] private Button button;
        [SerializeField] private TextMeshProUGUI amountLabel;

        [Header("Settings")]
        [SerializeField] private EquipmentSlotCategory slotCategory;
        [SerializeField] private int slotIndex;

        private Equipment _equipment;
        private IInventoryStorage _inventoryStorage;

        [Inject]
        private void Construct(PlayerController playerController) 
        {
            _equipment = playerController.PlayerInventory.Equipment;
            _inventoryStorage = playerController.PlayerInventory.InventoryStorage;
        }

        private void Awake()
        {
            button.onClick.AddListener(OnClick);

            SetIcon(slotCategory.Icon);
            amountLabel.gameObject.SetActive(false);
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

        private void HandleItemEquipped(InventoryItem item, int index)
        {
            if (Matches(item, index))
            {
                SetIcon(item.ItemBase.Icon);
                SetAmountLabel(item.Amount);
            }
        }

        private bool Matches(InventoryItem item, int index)
        {
            return item.ItemBase.EquipmentSlot == slotCategory && index == slotIndex;
        }

        private void Refresh()
        {
            var item = _equipment.GetEquipped(slotCategory, slotIndex);
            SetIcon(item != null ? item.ItemBase.Icon : slotCategory.Icon);
            SetAmountLabel(item != null ? item.Amount : 0);
        }

        private void SetIcon(Sprite icon)
        {
            this.icon.sprite = icon;
        }

        private void SetAmountLabel(int amount)
        {
            amountLabel.gameObject.SetActive(amount > 0);

            amountLabel.SetText(amount.ToString());
        }

        private void OnClick()
        {
            var item = _equipment.GetEquipped(slotCategory, slotIndex);

            if (_equipment.TryUnequipItem(slotCategory, slotIndex))
            {
                Refresh();
                _inventoryStorage.AddItemAndNotify(item.ItemBase, item.Amount);
            }
                
        }

        public InventoryItem Get()
        {
            var equippedItem = _equipment.GetEquipped(slotCategory, slotIndex);

            if (_equipment.TryUnequipItem(slotCategory, slotIndex))
            {
                Refresh();
            }

            return equippedItem;
        }

        public bool Drop(InventoryItem item)
        {
            if (item == null) return false;

            if (slotCategory != item.ItemBase.EquipmentSlot)
                return false;

            _equipment.TryEquipItem(item, slotIndex);

            return true;
        }
    }
}
