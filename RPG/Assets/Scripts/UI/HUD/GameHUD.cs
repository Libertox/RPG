using InputSystem;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI.HUD
{
    public class GameHUD : MonoBehaviour, IView
    {
        private InputManager _inputManager;

        [Inject]
        private void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public void Initialize()
        {

        }

        public void SubscribeToInputEvents()
        {
            _inputManager.EnableGameMap(true);
        }

        public void UnsubscribeToInputEvents()
        {
            _inputManager.EnableGameMap(false);
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
      
    }
}
