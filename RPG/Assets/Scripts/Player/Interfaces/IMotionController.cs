

using UnityEngine;

namespace Player
{
    public interface IMotionController
    {
        public Quaternion Rotation { get; }
        public Vector3 Position { get; }

        public void Move(Vector2 moveInput);
        public void Rotate(Vector3 rotationDirection);


    }
}
