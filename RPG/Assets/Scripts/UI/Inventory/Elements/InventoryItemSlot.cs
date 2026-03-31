using Entity.Player;
using InputSystem;
using InventorySystem;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.Inventory
{
    public class InventoryItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action<ItemConfigBase> OnSelected;
        public event Action OnDeselected;

        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amount;

        public ItemConfigBase Item { get; private set; }

        private PlayerInventory _playerInventory;
        private InputManager _inputManager;

        public RectTransform RectTransform => (RectTransform)transform;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager)
        {
            _playerInventory = playerController.PlayerData.Inventory;
            _inputManager = inputManager;
        }

        public InventoryItemSlot Initialize(ItemConfigBase item)
        {
            Item = item;

            icon.sprite = item.Icon;
            amount.SetText(_playerInventory.GetItemInventory(item).Amount.ToString());

            return this;
        }

        public InventoryItemSlot SetParent(Transform parent)
        {
            transform.SetParent(parent);

            return this;
        }

        public InventoryItemSlot SetAnchoredPosition(Vector2 anchoredPosition)
        {
            RectTransform.anchoredPosition = anchoredPosition;

            return this;
        }

        public InventoryItemSlot SetSize(Vector2 size)
        {
            RectTransform.sizeDelta = size;

            return this;
        }

        private void EquipItem()
        {
            _playerInventory.TryEquipItem(Item);
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
