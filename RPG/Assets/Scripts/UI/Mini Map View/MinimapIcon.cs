

using UnityEngine;
using UnityEngine.UI;

namespace UI.MinimapView
{
    public class MinimapIcon : MonoBehaviour
    {
        [SerializeField] private Image icon;

        public RectTransform RectTransform => transform as RectTransform; 

        public MinimapIcon SetIcon(Sprite icon)
        {
            this.icon.sprite = icon;
            return this;
        }

        public MinimapIcon SetSize(Vector2 size)
        {
            this.RectTransform.sizeDelta = size;
            return this;
        }

        public MinimapIcon SetPosition(Vector3 position)
        {
            RectTransform.anchoredPosition = position;
            return this;
        }

    }

}
