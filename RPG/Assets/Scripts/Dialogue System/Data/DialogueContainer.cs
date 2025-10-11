using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = nameof(DialogueContainer), menuName = "ScriptableObjects/DialogueSystem/"+nameof(DialogueContainer))]
    public class DialogueContainer : ScriptableObject
    {
        [field: SerializeField] public DialogueLine[] Dialogues;
    }

    [System.Serializable]
    public struct DialogueLine
    {
        public string Content;
        public DialogueActor DialogueActor;
    }

}

