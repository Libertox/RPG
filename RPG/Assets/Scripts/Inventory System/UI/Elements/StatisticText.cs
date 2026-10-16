using TMPro;
using UnityEngine;

namespace InventorySystem.UI
{
    public class StatisticText : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI statisticDescriptionText;
        [SerializeField] private TextMeshProUGUI comparisonResultText;

        [SerializeField] private Color betterComparisonColor;
        [SerializeField] private Color worseComparisonColor;

        public void UpdateDescription(string description) 
        {
            statisticDescriptionText.SetText(description);
        }

        public void UpdateComparisonResult(float value)
        {
            comparisonResultText.SetText($"{(value > 0 ? '+' : '-')}{value}");
            comparisonResultText.color = value > 0 ? betterComparisonColor : worseComparisonColor;
        }

        public void ActiveComparisonResult(bool active)
        {
            comparisonResultText.gameObject.SetActive(active);
        }

    }
}
