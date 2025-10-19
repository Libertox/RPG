

using System;
using UnityEngine;

namespace QuestSystem
{
    [CreateAssetMenu(fileName = nameof(QuestStep), menuName = "ScriptableObjects/Quest System/" + nameof(QuestStep))]
    public class QuestStep : ScriptableObject
    {
        public event Action OnStarted;
        public event Action OnCompleted;

        public bool IsActive { get; private set; }

        public void StartStep()
        {
            Debug.Log("Quest Step: " + name + " Started");

            OnStarted?.Invoke();

            IsActive = true;
        }

        public void FinishStep()
        {
            Debug.Log("Quest Step: " + name + " Finished");

            OnCompleted?.Invoke();

            IsActive = false;
        }
        

        public void Reset()
        {
            IsActive = false;
            OnStarted = null;
            OnCompleted = null;
        }

    }
}
