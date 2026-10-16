using Entity.Player;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;
using Utility;
using Zenject;

namespace InventorySystem.UI
{
    public class ItemInformationUI : UIElement<ItemInformationUI>
    {
        [Header("Base References")]
        [SerializeField] private TextMeshProUGUI itemNameText;
        [SerializeField] private TextMeshProUGUI categoryNameText;
        [SerializeField] private TextMeshProUGUI rarityNameText;

        [SerializeField] private TextMeshProUGUI weightText;
        [SerializeField] private TextMeshProUGUI goldText;

        [Header("Required Level References")]
        [SerializeField] private TextMeshProUGUI requiredLevelText;
        [SerializeField] private Color requiredLevelMetColor;
        [SerializeField] private Color requiredLevelNotMetColor;

        [Header("Statistic Refernces")]

        [SerializeField] private StatisticText mainStatsText;

        [SerializeField] private StatisticText statTextPrefab;
        [SerializeField] private Transform statsContainer;


        [Header("Comparison References")]
        [SerializeField] private GameObject comparisonInputLabel;

        private PlayerController playerController;
        private ComponentPool<StatisticText> additionalStatsPool;
        private List<StatisticText> displayedStatisticTexts = new();

        [Inject]
        private void Construct(PlayerController playerController)
        {
            this.playerController = playerController;
        }

        private void Awake()
        {
            additionalStatsPool = new(statTextPrefab, statsContainer);
        }

        public void Setup(InventorySlot item)
        {
            ItemConfigBase itemConfig = item.ItemConfig;

            itemNameText.SetText(itemConfig.Name);
            weightText.SetText(itemConfig.Weight.ToString());
            goldText.SetText(itemConfig.Gold.ToString());

            UpdateCategory(itemConfig.Category);
            UpdateRarity(itemConfig.Rarity);

            UpdateRequiredLevel(itemConfig);
            UpdateItemStats(item);
        }

        private void UpdateItemStats(InventorySlot item)
        {
            string mainStatDescription = item.MainModifier != null ? item.MainModifier.Config.GetDescription() : "";
            mainStatsText.UpdateDescription(mainStatDescription);

            foreach (var text in displayedStatisticTexts)
                additionalStatsPool.Release(text);

            displayedStatisticTexts.Clear();

            foreach (var modifier in item.AllModifiers)
            {
                var statText = additionalStatsPool.Get();

                statText.UpdateDescription(modifier.Config.GetDescription());
                displayedStatisticTexts.Add(statText);
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(statsContainer.transform as RectTransform);
        }

        private void UpdateRequiredLevel(ItemConfigBase itemConfig)
        {
            Color requiredLevelColor = itemConfig.RequiredLevel > playerController.Statistic.Level.Value ? requiredLevelNotMetColor : requiredLevelMetColor;
            string levelColor = ColorUtility.ToHtmlStringRGB(requiredLevelColor);
            requiredLevelText.SetText($"Required Level: <color=#{levelColor}>{itemConfig.RequiredLevel}</color>");
        }

        private void UpdateRarity(RarityCategory rarity)
        {
            rarityNameText.SetText(rarity.Name);
            rarityNameText.color = rarity.Color;
        }

        private void UpdateCategory(ItemCategory category)
        {
            categoryNameText.SetText(category.Name);
        }

        public void ActiveComparisonItemLabel(bool active)
        {
            comparisonInputLabel.SetActive(active);
        }
        public void SetupComparisonItems(InventorySlot currentItem, InventorySlot comparedItem)
        {
            UpdateMainStatComparison(currentItem, comparedItem);
            UpdateAdditionalStatComparisons(currentItem, comparedItem);
        }

        private void UpdateMainStatComparison(InventorySlot currentItem, InventorySlot comparedItem)
        {
            mainStatsText.ActiveComparisonResult(true);

            float currentValue = currentItem.MainModifier.GetModifiedValue(playerController.Statistic);
            float comparedValue = comparedItem.MainModifier.GetModifiedValue(playerController.Statistic);

            mainStatsText.UpdateComparisonResult(currentValue - comparedValue);
        }

        private void UpdateAdditionalStatComparisons(InventorySlot currentItem, InventorySlot comparedItem)
        {
            for (int i = 0; i < currentItem.AllModifiers.Count && i < displayedStatisticTexts.Count; i++)
            {
                var currentModifier = currentItem.AllModifiers[i];
                var comparedModifier = comparedItem.GetModifierByType(currentModifier.GetType());

                float currentValue = currentModifier.GetModifiedValue(playerController.Statistic);
                float comparedValue = comparedModifier != null
                    ? comparedModifier.GetModifiedValue(playerController.Statistic)
                    : 0f;

                displayedStatisticTexts[i].ActiveComparisonResult(true);
                displayedStatisticTexts[i].UpdateComparisonResult(currentValue - comparedValue);
            }
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

        public void HideComparisonResults()
        {
            mainStatsText.ActiveComparisonResult(false);

            foreach (var statText in displayedStatisticTexts)
                statText.ActiveComparisonResult(false);
        }

    }
}
