using DialogueSystem;
using InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

namespace UI.DialogueView
{
    public class DialogueView : UIViewBase
    {
        [SerializeField] private float typewriteAnimationDuration = 0.02f;

        [SerializeField] private TextMeshProUGUI dialogueActorName;
        [SerializeField] private TextMeshProUGUI dialogueContent;

        [SerializeField] private Image dialogueActorPotrait;

        [SerializeField] private Button continueButton;

        private DialogueManager dialogueManager;
        private InputManager inputManager;
        private UIViewManager uIViewManager;

        private Coroutine typingCoroutine;

        private TypewriterEffect typewriterEffect;

        [Inject]
        public void Construct(DialogueManager dialogueManager, InputManager inputManager, UIViewManager uIViewManager)
        {
            this.dialogueManager = dialogueManager;
            this.inputManager = inputManager;
            this.uIViewManager = uIViewManager;

            SubscribeDialogueEvents();
        }

        public override void Initialize()
        {
            continueButton.onClick.AddListener(OnContinueButtonClick);

            typewriterEffect = new TypewriterEffect(typewriteAnimationDuration);       
        }

        public void OpenView()
        {
            _= uIViewManager.TryOpenView(ViewID, true);
        }

        public async void OpenPreviewView()
        {
            await uIViewManager.OpenPreviousView();
        }

        public override void SubscribeToInputEvents()
        {
            inputManager.OnSubmitPressed += ShowNextDialogueLine;
            inputManager.OnContinuePressed += ShowNextDialogueLine;
        }

        public override void UnsubscribeToInputEvents()
        {
            inputManager.OnSubmitPressed -= ShowNextDialogueLine;
            inputManager.OnContinuePressed -= ShowNextDialogueLine;
        }

        private void SubscribeDialogueEvents()
        {
            dialogueManager.OnDialogueCompleted += OpenPreviewView;
            dialogueManager.OnDialgoueLineChanged += UpdateContent;
            dialogueManager.OnDialogueStarted += OpenView;
        }

        private void UnsubscribeDialogueEvents()
        {
            dialogueManager.OnDialogueCompleted -= OpenPreviewView;
            dialogueManager.OnDialgoueLineChanged -= UpdateContent;
            dialogueManager.OnDialogueStarted -= OpenView;
        }

    
        private void UpdateContent(DialogueLine dialogueLine)
        {
            if (typingCoroutine != null)
                StopCoroutine(typingCoroutine);

            dialogueActorPotrait.sprite = dialogueLine.DialogueActor.ActorPortrait;
            dialogueActorName.text = dialogueLine.DialogueActor.ActorName;

            continueButton.gameObject.SetActive(false);

            typingCoroutine = StartCoroutine(typewriterEffect.PlayAnimation(dialogueContent, dialogueLine.Content, OnTypewriteEffectCompleted));
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
                dialogueManager.ChangeDialogueLine();
            }
        }

        private bool TrySkipTypingAnimation()
        {
            if (!typewriterEffect.IsAnimationPlaying) return false;

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
