using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace InventorySystem.UI
{
    [RequireComponent(typeof(Toggle))]
    public class ItemFilterToggle : MonoBehaviour
    {
        [SerializeField] private InventorySortStrategy sortStrategy;

        private Toggle toggle;
        private SelectSortStrategySignal selectSortStrategySignal;

        [Inject]
        private void Construct(SelectSortStrategySignal selectSortStrategySignal)
        {
            this.selectSortStrategySignal = selectSortStrategySignal;
        }

        private void Awake()
        {
            toggle = GetComponent<Toggle>();
            toggle.onValueChanged.AddListener(SelectSortStrategy); 
        }

        public void SelectSortStrategy(bool isOn)
        {
            if (isOn)
                selectSortStrategySignal.Fire(sortStrategy);
        }

    }
}
