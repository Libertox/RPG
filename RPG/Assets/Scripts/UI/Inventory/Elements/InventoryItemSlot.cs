using Entity.Player;
using InputSystem;
using InventorySystem;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.Inventory
{
    public class InventoryItemSlot : UIElement<InventoryItemSlot>, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<ItemConfigBase> OnSelected;
        public event Action OnDeselected;

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amount;

        public ItemConfigBase Item { get; private set; }

        private PlayerInventory _playerInventory;
        private InputManager _inputManager;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager)
        {
            _playerInventory = playerController.PlayerInventory;
            _inputManager = inputManager;
        }

        public InventoryItemSlot Initialize(ItemConfigBase item)
        {
            Item = item;

            icon.sprite = item.Icon;
            amount.SetText(_playerInventory.InventoryStorage.FindInventoryItem(item).Amount.ToString());

            return this;
        }

        private void EquipItem()
        {
            _playerInventory.Equipment.TryEquipItem(_playerInventory.InventoryStorage.FindInventoryItem(Item));
        }

        private void DropItem()
        {
            _playerInventory.DropItem(Item);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnSelected?.Invoke(Item);

            _inputManager.OnLeftMouseClicked += EquipItem;
            _inputManager.OnRightMouseClicked += DropItem;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            OnDeselected?.Invoke();

            _inputManager.OnLeftMouseClicked -= EquipItem;
            _inputManager.OnRightMouseClicked -= DropItem;
        }

        private void OnDisable()
        {
            _inputManager.OnLeftMouseClicked -= EquipItem;
            _inputManager.OnRightMouseClicked -= DropItem;
        }


    }
}
