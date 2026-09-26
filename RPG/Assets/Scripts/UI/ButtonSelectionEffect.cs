using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ButtonSelectionEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float targetScale = 1.1f;
        [SerializeField] private float scaleDuration = 0.5f;

        private Vector3 startScale;

        private void Start()
        {
            startScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(targetScale, scaleDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(startScale, scaleDuration);
        }
    }
}
