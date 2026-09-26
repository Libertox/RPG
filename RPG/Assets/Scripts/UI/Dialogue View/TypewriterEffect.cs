using System;
using System.Collections;
using TMPro;
using UnityEngine;

namespace UI.DialogueView
{
    public class TypewriterEffect
    {
        public bool IsAnimationPlaying { get; private set; }

        private readonly WaitForSeconds effectSpeed;

        public TypewriterEffect(float effectDuration)
        {
            effectSpeed = new WaitForSeconds(effectDuration);
        }

        public IEnumerator PlayAnimation(TextMeshProUGUI target, string message, Action OnAnimationCompleted = null)
        {
            target.text = message;
            target.maxVisibleCharacters = 0;

            IsAnimationPlaying = true;

            while (target.maxVisibleCharacters < message.Length)
            {
                target.maxVisibleCharacters++;

                yield return effectSpeed;
            }

            OnAnimationCompleted?.Invoke();

            IsAnimationPlaying = false;
        }

    }
}
