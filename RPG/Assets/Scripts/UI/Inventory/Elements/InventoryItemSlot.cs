using InventorySystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Inventory
{
    public class InventoryItemSlot : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI amount;

        public ItemBase Item { get; private set; }

        private PlayerInventory _playerInventory;

        public RectTransform RectTransform => (RectTransform)transform;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(OnClick);
        }

        public InventoryItemSlot Initialize(ItemBase item, PlayerInventory playerInventory)
        {
            Item = item;
            _playerInventory = playerInventory;

            icon.sprite = item.Icon;
            amount.SetText(playerInventory.GetItemInventory(item).Amount.ToString());

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

        private void OnClick()
        {
            _playerInventory.TryEquipItem(Item);

        }


    }
}
