

using DG.Tweening;
using Entity.Player;
using InputSystem;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameOverView : UIViewBase
    {
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private float fadeDuration;

        private InputManager _inputManager;
        private PlayerController _playerController;
        private UIViewManager _viewManager;

        [Inject]
        private void Construct(InputManager inputManager, PlayerController playerController, UIViewManager viewManager)
        {
            _inputManager = inputManager;
            _playerController = playerController;
            _viewManager = viewManager;
        }

        public override void Initialize()
        {
            base.Initialize();

            canvasGroup.alpha = 0f;
            _playerController.OnDie += OnPlayerDie;
        }

        public async void OnPlayerDie()
        {
            await _viewManager.TryOpenView(ViewID);  
        }

        public override void SubscribeToInputEvents()
        {
            _inputManager.EnableUIMap(true);
        }

        public override void UnsubscribeToInputEvents()
        {
            _inputManager.EnableUIMap(false);
        }

        public override void Open()
        {
            base.Open();
            canvasGroup.alpha = 1f;
        }

        public override Task OpenAsync()
        {
            base.Open();

            return canvasGroup.DOFade(1f, fadeDuration).AsyncWaitForCompletion();
        }

        public override void Close()
        {
            base.Close();
            canvasGroup.alpha = 0f;
        }

        public override Task CloseAsync()
        {
            return canvasGroup.DOFade(0f, fadeDuration).AsyncWaitForCompletion();
        }
    }
}
