

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

            return new Vector3(randomX, _centerPosition.y, randomZ);
        }

        private Vector3 GetMinRectanglePosition()
        {
            float minX = _centerPosition.x - _width;
            float minZ = _centerPosition.z - _height;

            return new Vector3(minX, _centerPosition.y, minZ);
        }

        private Vector3 GetMaxRectanglePosition()
        {
            float maxX = _centerPosition.x + _width;
            float maxZ = _centerPosition.z + _height;

            return new Vector3(maxX, _centerPosition.y, maxZ);
        }

        public override void DrawArea(Vector3 centerPosition, Color lineColor)
        {
            _centerPosition = centerPosition;

            Vector3 rightUpperCorner = GetMaxRectanglePosition();
            Vector3 leftDownCorner = GetMinRectanglePosition();
            Vector3 leftUpperCorner = new Vector3(leftDownCorner.x, _centerPosition.y,  rightUpperCorner.z);
            Vector3 rightDownCorner = new Vector3(rightUpperCorner.x, _centerPosition.y, leftDownCorner.z);

            Debug.DrawLine(rightUpperCorner, leftUpperCorner, lineColor);
            Debug.DrawLine(rightUpperCorner, rightDownCorner, lineColor);
            Debug.DrawLine(rightDownCorner, leftDownCorner, lineColor);
            Debug.DrawLine(leftDownCorner, leftUpperCorner, lineColor);
        }

    }
}
