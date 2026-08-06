using InputSystem;
using Zenject;
using UnityEngine;

namespace UI.HUD
{
    public class GameHUD : UIViewBase
    {
        [SerializeField] private UIViewSO inventoryViewID;

        private InputManager _inputManager;
        private UIViewManager _viewManager;

        [Inject]
        private void Construct(InputManager inputManager, UIViewManager viewManager)
        {
            _inputManager = inputManager;
            _viewManager = viewManager;
        }

     
        public override void SubscribeToInputEvents()
        {
            _inputManager.EnableGameMap(true);

            _inputManager.OnInventoryButtonPressed += OpenInventoryView;
        }

      
        public override void UnsubscribeToInputEvents()
        {
            _inputManager.EnableGameMap(false);

            _inputManager.OnInventoryButtonPressed -= OpenInventoryView;
        }

        private async void OpenInventoryView()
        {
            await _viewManager.TryOpenView(inventoryViewID, true);
        }


    }
}
