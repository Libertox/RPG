using DialogueSystem;
using InputSystem;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.DialogueView
{
    public class DialogueView : MonoBehaviour, IView
    {
        [SerializeField] private float typewriteAnimationDuration = 0.02f;

        [SerializeField] private TextMeshProUGUI dialogueActorName;
        [SerializeField] private TextMeshProUGUI dialogueContent;

        [SerializeField] private Image dialogueActorPotrait;

        [SerializeField] private Button continueButton;

        private DialogueManager _dialogueManager;
        private InputManager _inputManager;
        private UIViewManager _uIViewManager;

        private Coroutine _typingCoroutine;

        private TypewriterEffect _typewriterEffect;

        [Inject]
        public void Construct(DialogueManager dialogueManager, InputManager inputManager, UIViewManager uIViewManager)
        {
            _dialogueManager = dialogueManager;
            _inputManager = inputManager;
            _uIViewManager = uIViewManager;

            SubscribeDialogueEvents();
        }

        public void Initialize()
        {
            continueButton.onClick.AddListener(OnContinueButtonClick);

            _typewriterEffect = new TypewriterEffect(typewriteAnimationDuration);
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

        public async void OpenPreviewView()
        {
            await _uIViewManager.OpenPreviousView();
        }

        public void SubscribeToInputEvents()
        {
            _inputManager.OnSubmitUIButtonPressed += ShowNextDialogueLine;
            _inputManager.OnContinueUIButtonPressed += ShowNextDialogueLine;
        }

        public void UnsubscribeToInputEvents()
        {
            _inputManager.OnSubmitUIButtonPressed -= ShowNextDialogueLine;
            _inputManager.OnContinueUIButtonPressed -= ShowNextDialogueLine;
        }

        private void SubscribeDialogueEvents()
        {
            _dialogueManager.OnDialogueCompleted += OpenPreviewView;
            _dialogueManager.OnDialgoueLineChanged += UpdateContent;
        }

        private void UnsubscribeDialogueEvents()
        {
            _dialogueManager.OnDialogueCompleted -= OpenPreviewView;
            _dialogueManager.OnDialgoueLineChanged -= UpdateContent;
        }

    
        private void UpdateContent(DialogueLine dialogueLine)
        {
            if (_typingCoroutine != null)
                StopCoroutine(_typingCoroutine);

            dialogueActorPotrait.sprite = dialogueLine.DialogueActor.ActorPortrait;
            dialogueActorName.text = dialogueLine.DialogueActor.ActorName;

            continueButton.gameObject.SetActive(false);

            _typingCoroutine = StartCoroutine(_typewriterEffect.PlayAnimation(dialogueContent, dialogueLine.Content, OnTypewriteEffectCompleted));
        }

        private void OnTypewriteEffectCompleted()
        {
            continueButton.gameObject.SetActive(true);
        }

        private void OnContinueButtonClick()
        {
            EventSystem.current.SetSelectedGameObject(null);
            ShowNextDialogueLine();
        }

        private void ShowNextDialogueLine()
        {
            if (!TrySkipTypingAnimation())
            {
                _dialogueManager.ChangeToNextDialogueLine();
            }
        }

        private bool TrySkipTypingAnimation()
        {
            if (!_typewriterEffect.IsAnimationPlaying) return false;

            dialogueContent.maxVisibleCharacters = dialogueContent.text.Length;

            return true;
        }
     
        private void OnDestroy()
        {
            UnsubscribeDialogueEvents();
            UnsubscribeToInputEvents();
        }

       
    }
}
