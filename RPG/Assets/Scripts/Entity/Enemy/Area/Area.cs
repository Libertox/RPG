

using UnityEngine;

namespace Area
{
    public abstract class Area
    {
        protected Vector3 centerPosition;

        public Area(Vector3 centerPosition)
        {
            this.centerPosition = centerPosition;
        }

        public abstract Vector3 GetRandomPositionWithin();
        public abstract void DrawArea(Vector3 centerPosition, Color lineColor);
    }
}
