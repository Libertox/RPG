

using DG.Tweening;
using Entity.Player;
using InputSystem;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace UI
{
    public class GameOverView : MonoBehaviour, IView
    {
        [SerializeField] private CanvasGroup canvasGroup;

        [SerializeField] private float fadeDuration;

        private InputManager _inputManager;

        [Inject]
        private void Construct(InputManager inputManager)
        {
            _inputManager = inputManager;
        }

        public void Initialize()
        {
            canvasGroup.alpha = 0f;
        }

        public void SubscribeToInputEvents()
        {
            _inputManager.EnableUIMap(true);
        }

        public void UnsubscribeToInputEvents()
        {
            _inputManager.EnableUIMap(false);
        }

        public void Open()
        {
            gameObject.SetActive(true);

            canvasGroup.alpha = 1f;
        }

        public Task OpenAsync()
        {
            gameObject.SetActive(true);

            return canvasGroup.DOFade(1f, fadeDuration).AsyncWaitForCompletion();
        }

        public void Close()
        {
            canvasGroup.alpha = 0f;
        }

        public Task CloseAsync()
        {
            return canvasGroup.DOFade(0f, fadeDuration).AsyncWaitForCompletion();
        }
    }
}
