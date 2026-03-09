using InputSystem;
using System.Threading.Tasks;
using UI.Inventory;
using UnityEngine;
using Zenject;

namespace UI.HUD
{
    public class GameHUD : MonoBehaviour, IView
    {
        private InputManager _inputManager;
        private UIViewManager _viewManager;

        [Inject]
        private void Construct(InputManager inputManager, UIViewManager viewManager)
        {
            _inputManager = inputManager;
            _viewManager = viewManager;
        }

        public void Initialize()
        {

        }

        public void SubscribeToInputEvents()
        {
            _inputManager.EnableGameMap(true);

            _inputManager.OnInventoryButtonPressed += OpenInventoryView;
        }

      
        public void UnsubscribeToInputEvents()
        {
            _inputManager.EnableGameMap(false);

            _inputManager.OnInventoryButtonPressed -= OpenInventoryView;
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public Task OpenAsync()
        {
            Open();

            return Task.CompletedTask;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }

        public Task CloseAsync()
        {
            Close();

            return Task.CompletedTask;
        }

        private async void OpenInventoryView()
        {
            await _viewManager.TryOpenView<InventoryView>(true);
        }


    }
}
