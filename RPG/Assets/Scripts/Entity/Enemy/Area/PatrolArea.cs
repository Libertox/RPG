

using UnityEngine;
using Utility.Attribute;

namespace Area
{
    public class PatrolArea : MonoBehaviour
    {
        [SerializeField] private AreaType areaType;

        [SerializeField, ShowIf(nameof(isRectangle))] private float width;
        [SerializeField, ShowIf(nameof(isRectangle))] private float height;

        [SerializeField, ShowIf(nameof(isCircle))] private float radius;

        [SerializeField, HideInInspector] private bool isRectangle;
        [SerializeField, HideInInspector] private bool isCircle;

        private Area _patrolArea;

        private void OnValidate()
        {
            isRectangle = areaType == AreaType.Rectangle;
            isCircle = areaType == AreaType.Circle;

            if (isRectangle)
                _patrolArea = new RectangleArea(transform.position, width, height);
            else if(isCircle)
                _patrolArea = new CircleArea(transform.position, radius);
        }

        public Vector3 GetRandomPositionWithin()
        {
            return _patrolArea.GetRandomPositionWithin();
        }

        private void OnDrawGizmosSelected()
        {
            _patrolArea.DrawArea(transform.position, Color.white);
        }
    }
}
