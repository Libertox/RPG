

using UnityEngine;
using UnityEngine.UI;

namespace InteractionPromptSystem.Presentation
{
    public class PromptIcon : MonoBehaviour
    {
        [SerializeField] private Image icon;
        [SerializeField] private CanvasGroup canvasGroup;

        private Camera mainCamera;
        private Sprite baseIcon;

        public PromptIcon SetBaseIcon(Sprite baseIcon)
        {
            this.baseIcon = baseIcon;
            icon.sprite = baseIcon;
            return this;
        }

        public PromptIcon SetWidth(float width)
        {
            RectTransform rectTransform = icon.transform as RectTransform;
            rectTransform.sizeDelta = new(width, rectTransform.sizeDelta.y);
            return this;
        }

        public PromptIcon SetHeight(float height)
        {
            RectTransform rectTransform = icon.transform as RectTransform;
            rectTransform.sizeDelta = new(rectTransform.sizeDelta.x, height);
            return this;
        }


        public void SetIcon(Sprite icon)
        {
            this.icon.sprite = icon;
        }

        public void ResetIcon()
        {
            this.icon.sprite = baseIcon;
        }

        public void SetVisibility(float visible)
        {
            canvasGroup.alpha = visible;
        }

        public void LookAtCameraPosition()
        {
            if(!mainCamera)
                mainCamera = Camera.main;

            icon.transform.LookAt(mainCamera.transform.position, Vector3.down);
        }

    }
}
