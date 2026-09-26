

using UnityEngine;

namespace Area
{
    public class CircleArea : Area
    {
        private readonly float radius;

        public CircleArea(Vector3 centerPosition,  float radius) : base(centerPosition)
        {
            this.radius = radius;
        }

        public override Vector3 GetRandomPositionWithin()
        {
            float randomDistance = UnityEngine.Random.Range(0, radius);
            float randomAngle = UnityEngine.Random.Range(0, 360);

            Vector3 pointPosition = new Vector3(Mathf.Sin(Mathf.Deg2Rad * randomAngle), 0, Mathf.Cos(Mathf.Deg2Rad * randomAngle));

            pointPosition *= randomDistance;

            pointPosition += centerPosition;

            return pointPosition;
        }

        public override void DrawArea(Vector3 centerPosition, Color lineColor)
        {
            for(int i = 0; i < 359; i++)
            {
                Vector3 pointPosition = new Vector3(Mathf.Sin(Mathf.Deg2Rad * i), 0, Mathf.Cos(Mathf.Deg2Rad * i));
                Vector3 nextPointPosition = new Vector3(Mathf.Sin(Mathf.Deg2Rad * (i + 1)), 0, Mathf.Cos(Mathf.Deg2Rad * (i + 1)));

                pointPosition *= radius;
                nextPointPosition *= radius;

                pointPosition += centerPosition;
                nextPointPosition += centerPosition;

                Debug.DrawLine(pointPosition, nextPointPosition, lineColor);
            }
        }
    }
}
