

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

        private Area patrolArea;

        private void OnValidate()
        {
            isRectangle = areaType == AreaType.Rectangle;
            isCircle = areaType == AreaType.Circle;

            if (isRectangle)
                patrolArea = new RectangleArea(transform.position, width, height);
            else if(isCircle)
                patrolArea = new CircleArea(transform.position, radius);
        }

        public Vector3 GetRandomPositionWithin()
        {
            return patrolArea.GetRandomPositionWithin();
        }

        private void OnDrawGizmosSelected()
        {
            patrolArea.DrawArea(transform.position, Color.white);
        }
    }
}
