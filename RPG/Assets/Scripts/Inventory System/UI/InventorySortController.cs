using InputSystem;
using InventorySystem.UI;
using UnityEngine;
using Zenject;

namespace InventorySystem
{
    public class InventorySortController : MonoBehaviour
    {
        [SerializeField] private InventoryCategorySelector inventoryCategorySelector;

        [SerializeField] private InventorySortStrategy defaultSortStrategy;

        private InputManager inputManager;
        private SelectSortStrategySignal selectSortStrategySignal;

        private InventorySortStrategy currentSortStrategy;

        [Inject]
        private void Construct(InputManager inputManager, SelectSortStrategySignal selectSortStrategySignal)
        {
            this.inputManager = inputManager;
            this.selectSortStrategySignal = selectSortStrategySignal;

            selectSortStrategySignal += SetCurrentSortStrategy;
        }

        private void Awake()
        {
            currentSortStrategy = defaultSortStrategy;
        }

        public void SubscribeInputEvents()
        {
            inputManager.OnSortItemsPressed += SortCurrentGrid;
        }

        public void UnsubscribeInputEvents()
        {
            inputManager.OnSortItemsPressed -= SortCurrentGrid;
        }

        private void SetCurrentSortStrategy(InventorySortStrategy inventorySortStrategy)
        {
            currentSortStrategy = inventorySortStrategy;
        }

        public void SortCurrentGrid()
        {
            inventoryCategorySelector.GetCurrentGrid().Sort(currentSortStrategy);
        }

        private void OnDestroy()
        {
            selectSortStrategySignal -= SetCurrentSortStrategy;
        }
    }
}
