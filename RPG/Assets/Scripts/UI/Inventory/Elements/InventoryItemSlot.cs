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
    public class InventoryItemSlot : UIElement<InventoryItemSlot>, IDragable, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<ItemInventory> OnSelected;
        public event Action OnDeselected;

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amount;

        public ItemInventory Item { get; private set; }

        private PlayerInventory _playerInventory;
        private InputManager _inputManager;

        private ItemHolder _itemHolder;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager)
        {
            _playerInventory = playerController.PlayerInventory;
            _inputManager = inputManager;
        }

        public InventoryItemSlot Initialize(ItemInventory item, ItemHolder itemHolder)
        {
            Item = item;

            icon.sprite = item.ItemBase.Icon;
            amount.SetText(item.Amount.ToString());

            _itemHolder = itemHolder;

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

        public void Drop()
        {
            
        }

        public void Drag()
        {
            _itemHolder.SetItem(Item, this);
        }
    }
}
