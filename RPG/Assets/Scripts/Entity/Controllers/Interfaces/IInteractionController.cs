

namespace Entity
{
    [DefaultController(typeof(DefaultInteractionController))]
    public interface IInteractionController : IController
    {
        public void TryInteractWithInteractableObject();

    }

    public class DefaultInteractionController : IInteractionController
    {
        public void TryInteractWithInteractableObject()
        {
            
        }
    }
}
