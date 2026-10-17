using InputSystem;
using UI;
using Zenject;
using UnityEngine;

namespace InventorySystem.UI
{
    public class ItemsFilteringView : UIViewBase
    {
        [SerializeField] private InventorySortController sortController;

        private InputManager inputManager;
        private UIViewManager viewManager;

        [Inject]
        private void Construct(InputManager inputManager, UIViewManager viewManager)
        {
            this.inputManager = inputManager;
            this.viewManager = viewManager;
        }

        public override void SubscribeToInputEvents()
        {
            inputManager.EnableUIActions(true);

            inputManager.OnFilterItemsPressed += OpenPreviousView;
            inputManager.OnCancelPressed += OpenPreviousView;
        }

        public override void UnsubscribeToInputEvents()
        {
            inputManager.EnableUIActions(false);

            inputManager.OnFilterItemsPressed -= OpenPreviousView;
            inputManager.OnCancelPressed -= OpenPreviousView;
        }

        public override void Close()
        {
            base.Close();

            sortController.SortCurrentGrid();
        }

        private void OpenPreviousView()
        {
            _= viewManager.OpenPreviousView();
        }

     
    }
}
