

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

        private InputManager inputManager;
        private PlayerController playerController;
        private UIViewManager viewManager;

        [Inject]
        private void Construct(InputManager inputManager, PlayerController playerController, UIViewManager viewManager)
        {
            this.inputManager = inputManager;
            this.playerController = playerController;
            this.viewManager = viewManager;
        }

        public override void Initialize()
        {
            base.Initialize();

            canvasGroup.alpha = 0f;
            playerController.OnDie += OnPlayerDie;
        }

        public async void OnPlayerDie()
        {
            await viewManager.TryOpenView(ViewID);  
        }

        public override void SubscribeToInputEvents()
        {
            inputManager.EnableUIActions(true);
        }

        public override void UnsubscribeToInputEvents()
        {
            inputManager.EnableUIActions(false);
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
