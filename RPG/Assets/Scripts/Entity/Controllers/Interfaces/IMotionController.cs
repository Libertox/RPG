

using UnityEngine;

namespace Entity
{
    [DefaultController(typeof(DefaultMotionController))]
    public interface IMotionController : IController
    {
        public bool IsMoving { get; }
        public void Move();
        public void Rotate(Vector3 rotationDirection);


    }

    public class DefaultMotionController : IMotionController
    {
        public bool IsMoving => true;

        public void Move()
        {
            
        }

        public void Rotate(Vector3 rotationDirection)
        {
           
        }
    }
}
