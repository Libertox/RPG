using Entity.Player;
using InputSystem;
using InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.Inventory
{
    public class InventoryItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amount;

        public ItemBase Item { get; private set; }

        private PlayerInventory _playerInventory;
        private InputManager _inputManager;

        public RectTransform RectTransform => (RectTransform)transform;

        [Inject]
        public void Construct(PlayerController playerController, InputManager inputManager)
        {
            _playerInventory = playerController.PlayerData.Inventory;
            _inputManager = inputManager;
        }

        public InventoryItemSlot Initialize(ItemBase item)
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
            _inputManager.OnLeftMouseClicked += EquipItem;
            _inputManager.OnRightMouseClicked += DropItem;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
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
