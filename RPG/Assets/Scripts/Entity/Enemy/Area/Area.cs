

using UnityEngine;

namespace Area
{
    public abstract class Area
    {
        protected Vector3 _centerPosition;

        public Area(Vector3 centerPosition)
        {
            _centerPosition = centerPosition;
        }

        public abstract Vector3 GetRandomPositionWithin();
        public abstract void DrawArea(Vector3 centerPosition, Color lineColor);
    }
}
