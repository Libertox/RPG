

using UnityEngine;

namespace Area
{
    public class RectangleArea : Area
    {
        private readonly float _width;
        private readonly float _height;

        public RectangleArea(Vector3 centerPosition, float width, float height) : base(centerPosition)
        {
            _width = width;
            _height = height;
        }

        public override Vector3 GetRandomPositionWithin()
        {
            Vector3 maxPosition = GetMaxRectanglePosition();
            Vector3 minPosition = GetMinRectanglePosition();
         
            float randomX = UnityEngine.Random.Range(minPosition.x, maxPosition.x);
            float randomZ = UnityEngine.Random.Range(minPosition.z, maxPosition.z);

            return new Vector3(randomX, centerPosition.y, randomZ);
        }

        private Vector3 GetMinRectanglePosition()
        {
            float minX = centerPosition.x - _width;
            float minZ = centerPosition.z - _height;

            return new Vector3(minX, centerPosition.y, minZ);
        }

        private Vector3 GetMaxRectanglePosition()
        {
            float maxX = centerPosition.x + _width;
            float maxZ = centerPosition.z + _height;

            return new Vector3(maxX, centerPosition.y, maxZ);
        }

        public override void DrawArea(Vector3 centerPosition, Color lineColor)
        {
            base.centerPosition = centerPosition;

            Vector3 rightUpperCorner = GetMaxRectanglePosition();
            Vector3 leftDownCorner = GetMinRectanglePosition();
            Vector3 leftUpperCorner = new Vector3(leftDownCorner.x, base.centerPosition.y,  rightUpperCorner.z);
            Vector3 rightDownCorner = new Vector3(rightUpperCorner.x, base.centerPosition.y, leftDownCorner.z);

            Debug.DrawLine(rightUpperCorner, leftUpperCorner, lineColor);
            Debug.DrawLine(rightUpperCorner, rightDownCorner, lineColor);
            Debug.DrawLine(rightDownCorner, leftDownCorner, lineColor);
            Debug.DrawLine(leftDownCorner, leftUpperCorner, lineColor);
        }

    }
}
