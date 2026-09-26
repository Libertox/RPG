using InputSystem;
using Zenject;
using UnityEngine;

namespace UI.HUD
{
    public class GameHUD : UIViewBase
    {
        [SerializeField] private UIViewSO inventoryViewID;

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
            inputManager.EnablePlayerActions(true);

            inputManager.OnInventoryPressed += OpenInventoryView;
        }

      
        public override void UnsubscribeToInputEvents()
        {
            inputManager.EnablePlayerActions(false);

            inputManager.OnInventoryPressed -= OpenInventoryView;
        }

        private async void OpenInventoryView()
        {
            await viewManager.TryOpenView(inventoryViewID, true);
        }


    }
}
