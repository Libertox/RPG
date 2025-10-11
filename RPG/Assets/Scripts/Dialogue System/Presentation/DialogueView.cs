using InputSystem;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace DialogueSystem.Presentation
{
    public class DialogueView : MonoBehaviour
    {
        [SerializeField] private float typewriteAnimationDuration = 0.02f;

        [SerializeField] private TextMeshProUGUI dialogueActorName;
        [SerializeField] private TextMeshProUGUI dialogueContent;

        [SerializeField] private Image dialogueActorPotrait;

        [SerializeField] private Button continueButton;

        private DialogueManager _dialogueManager;
        private InputManager _inputManager;

        private Coroutine _typingCoroutine;

        private TypewriterEffect _typewriterEffect;

        [Inject]
        public void Construct(DialogueManager dialogueManager, InputManager inputManager)
        {
            _dialogueManager = dialogueManager;
            _inputManager = inputManager;

            SubscribeDialogueEvents();
        }

        private void Awake()
        {
            continueButton.onClick.AddListener(OnContinueButtonClick);

            _typewriterEffect = new TypewriterEffect(typewriteAnimationDuration);
        }

        private void SubscribeDialogueEvents()
        {
            _dialogueManager.OnDialogueStarted += Open;
            _dialogueManager.OnDialogueCompleted += Hide;
            _dialogueManager.OnDialgoueLineChanged += UpdateContent;
        }

        private void UnsubscribeDialogueEvents()
        {
            _dialogueManager.OnDialogueStarted -= Open;
            _dialogueManager.OnDialogueCompleted -= Hide;
            _dialogueManager.OnDialgoueLineChanged -= UpdateContent;
        }

        private void SubscribeInputEvents()
        {
            _inputManager.InputEvents.OnSubmitButtonPressed += ShowNextDialogueLine;
            _inputManager.InputEvents.OnContinueButtonPressed += ShowNextDialogueLine;
        }

        private void UnsubscribeInputEvents()
        {
            _inputManager.InputEvents.OnSubmitButtonPressed -= ShowNextDialogueLine;
            _inputManager.InputEvents.OnContinueButtonPressed -= ShowNextDialogueLine;
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

        private void Open()
        {
            SubscribeInputEvents();
            gameObject.SetActive(true);
        }

        private void Hide()
        {
            UnsubscribeInputEvents();
            gameObject.SetActive(false);
        }

        private void OnDestroy()
        {
            UnsubscribeDialogueEvents();
            UnsubscribeInputEvents();
        }
    }
}
