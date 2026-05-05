using UnityEngine;

namespace UI
{
    public class UIElement<T> : MonoBehaviour where T : UIElement<T>
    {
        public RectTransform RectTransform => (RectTransform)transform;

        public T SetParent(Transform parent)
        {
            transform.SetParent(parent);

            return (T)this;
        }

        public T SetAnchoredPosition(Vector2 anchoredPosition)
        {
            RectTransform.anchoredPosition = anchoredPosition;

            return (T)this;
        }

        public T SetSize(Vector2 size)
        {
            RectTransform.sizeDelta = size;

            return (T)this;
        }

        public Vector3 GetRightBottomCornerPosition()
        {
            Vector3 localCorner = new Vector3(RectTransform.rect.width * (1 - RectTransform.pivot.x), -RectTransform.rect.height * RectTransform.pivot.y, 0);

            return RectTransform.TransformPoint(localCorner);
        }
    }
}
