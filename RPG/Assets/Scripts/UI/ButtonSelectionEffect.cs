using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class ButtonSelectionEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private float _targetScale;
        [SerializeField] private float _scaleDuration;

        private Vector3 _startScale;

        private void Start()
        {
            _startScale = transform.localScale;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            transform.DOScale(_targetScale, _scaleDuration);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            transform.DOScale(_startScale, _scaleDuration);
        }
    }
}
