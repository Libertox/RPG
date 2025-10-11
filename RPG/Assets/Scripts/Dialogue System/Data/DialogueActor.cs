

using UnityEngine;

namespace DialogueSystem
{
    [CreateAssetMenu(fileName = nameof(DialogueActor), menuName = "ScriptableObjects/DialogueSystem/"+nameof(DialogueActor))]
    public class DialogueActor : ScriptableObject
    {
        [field: SerializeField] public Sprite ActorPortrait { get; private set; }
        [field: SerializeField] public string ActorName { get; private set; }

    }

}
