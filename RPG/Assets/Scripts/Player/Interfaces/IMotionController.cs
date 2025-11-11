

using UnityEngine;

namespace Player
{
    public interface IMotionController
    {
        public void Move();
        public void Rotate(Vector3 rotationDirection);


    }
}
