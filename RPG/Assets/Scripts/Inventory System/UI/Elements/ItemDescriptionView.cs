using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem.UI
{
    public class ItemDescriptionView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI categoryName;
        [SerializeField] private TextMeshProUGUI rarityName;

        [SerializeField] private TextMeshProUGUI requiredLevelLabel;
        [SerializeField] private TextMeshProUGUI weightLabel;
        [SerializeField] private TextMeshProUGUI goldLabel;

        private SelectInventorySlotSignal _selectInventorySlotSignal;
        private DeselectInventorySlotSignal _deselectInventorySlotSignal;

        [Inject]
        private void Construct(SelectInventorySlotSignal selectInventorySlotSignal, DeselectInventorySlotSignal deselectInventorySlotSignal)
        {
            _selectInventorySlotSignal = selectInventorySlotSignal;
            _deselectInventorySlotSignal = deselectInventorySlotSignal;

            _selectInventorySlotSignal.Listen(OnItemSlotSelected);
            _deselectInventorySlotSignal.Listen(Hide);
        }

        private void OnItemSlotSelected(InventorySlotUI slot)
        {
            ShowAtPosition(slot.GetRightBottomCornerPosition());
            Setup(slot.Item.ItemBase);

            LayoutRebuilder.ForceRebuildLayoutImmediate(transform.transform as RectTransform);
        }

        public void Setup(ItemConfigBase item)
        {
            itemName.SetText(item.Name);
            categoryName.SetText(item.Category.Name);
            rarityName.SetText(item.Rarity.Name);
            rarityName.color = item.Rarity.Color;

            requiredLevelLabel.SetText($"Required Level: {item.RequiredLevel}");
            weightLabel.SetText(item.Weight.ToString());
            goldLabel.SetText(item.Gold.ToString());
        }

        public void ShowAtPosition(Vector3 worldPosition)
        {
            gameObject.SetActive(true);

            RectTransform rectTransform = (RectTransform)transform;

            RectTransform parentRect = rectTransform.parent as RectTransform;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                RectTransformUtility.WorldToScreenPoint(null, worldPosition),
                null,
                out Vector2 localPoint
            );

            rectTransform.anchoredPosition = localPoint;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            _selectInventorySlotSignal.Unlisten(OnItemSlotSelected);
            _deselectInventorySlotSignal.Unlisten(Hide);
        }
    }
}
