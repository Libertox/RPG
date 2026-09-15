using Entity.Player;
using InputSystem;
using System;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem.UI
{
    public class InventorySlotUI : UIElement<InventorySlotUI>, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amount;
        [SerializeField] private Image background;
        [SerializeField] private GameObject paddlockIcon;

        public InventorySlot Item { get; private set; }

        private PlayerInventory _playerInventory;
        private PlayerController _playerController;
        private InputManager _inputManager;
        private SelectInventorySlotSignal _selectInventorySlotSignal;
        private DeselectInventorySlotSignal _deselectInventorySlotSignal;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager, 
            SelectInventorySlotSignal selectInventorySlot, DeselectInventorySlotSignal deselectInventorySlot)
        {
            _playerInventory = playerController.PlayerInventory;
            _inputManager = inputManager;
            _playerController = playerController;

            _selectInventorySlotSignal = selectInventorySlot;
            _deselectInventorySlotSignal = deselectInventorySlot;
        }

        public InventorySlotUI Initialize(InventorySlot item)
        {
            Item = item;

            icon.sprite = item.ItemBase.Icon;
            background.sprite = item.ItemBase.Rarity.Presentation;
            amount.SetText(item.Amount.ToString());
            amount.gameObject.SetActive(item.ItemBase.CanStack);

            paddlockIcon.SetActive(item.ItemBase.RequiredLevel > _playerController.Level);

            return this;
        }

        private void EquipItem()
        {
            if (_playerInventory.Equipment.TryEquipItem(Item))
            {
                _deselectInventorySlotSignal.Fire();
            }
        }

        private void DropItem()
        {
            _playerInventory.DropItem(Item);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!gameObject.activeSelf) return;

            _selectInventorySlotSignal.Fire(this);

            _inputManager.OnLeftMouseClicked += EquipItem;
            _inputManager.OnRightMouseClicked += DropItem;
        }


        public void OnPointerExit(PointerEventData eventData)
        {
            _deselectInventorySlotSignal.Fire();

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
