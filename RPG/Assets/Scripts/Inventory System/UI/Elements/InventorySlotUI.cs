using Entity.Player;
using InputSystem;
using UI;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem.UI
{
    public class InventorySlotUI : UIElement<InventorySlotUI>, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<InventorySlotUI> OnSelected;
        public event Action OnDeselected;

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amount;

        public InventorySlot Item { get; private set; }

        private PlayerInventory _playerInventory;
        private InputManager _inputManager;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager)
        {
            _playerInventory = playerController.PlayerInventory;
            _inputManager = inputManager;
        }

        public InventorySlotUI Initialize(InventorySlot item)
        {
            Item = item;

            icon.sprite = item.ItemBase.Icon;
            amount.SetText(item.Amount.ToString());
            amount.gameObject.SetActive(item.ItemBase.CanStack);

            return this;
        }

        private void EquipItem()
        {
            _playerInventory.Equipment.TryEquipItem(Item);
        }

        private void DropItem()
        {
            _playerInventory.DropItem(Item);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!gameObject.activeSelf) return;

            OnSelected?.Invoke(this);

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
