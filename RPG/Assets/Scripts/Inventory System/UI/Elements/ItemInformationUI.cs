

using TMPro;
using UI;
using UnityEngine;

namespace InventorySystem.UI
{
    public class ItemInformationUI : UIElement<ItemInformationUI>
    {
        [Header("Item Information Refernces")]
        [SerializeField] private TextMeshProUGUI itemName;
        [SerializeField] private TextMeshProUGUI categoryName;
        [SerializeField] private TextMeshProUGUI rarityName;

        [SerializeField] private TextMeshProUGUI requiredLevelLabel;
        [SerializeField] private TextMeshProUGUI weightLabel;
        [SerializeField] private TextMeshProUGUI goldLabel;

        [Header("Comparison References")]
        [SerializeField] private GameObject comparisonItemLabel;
        [SerializeField] private TextMeshProUGUI comparisonValue;
        [SerializeField] private Color betterComparisonColor;
        [SerializeField] private Color worseComparisonColor;


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

        public void ActiveComparisonItemLabel(bool active)
        {
            comparisonItemLabel.SetActive(active);
        }

        public void UpdateComparisonResult(float value)
        {
            comparisonValue.SetText($"{(value > 0 ? '+' : '-')}{value}");
            comparisonValue.color = value > 0 ? betterComparisonColor : worseComparisonColor;
        }

        public void ActiveComparisonResult(bool active)
        {
            comparisonValue.gameObject.SetActive(active);
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

    }
}
