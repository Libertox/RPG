

namespace Entity
{
    [DefaultController(typeof(DefaultCombatController))]
    public interface ICombatController : IController
    {
        public bool IsAttacking { get; }

        public void Attack();
        public void SetIsAttacking(bool isAttacking);

    }

    public class DefaultCombatController : ICombatController
    {
        public bool IsAttacking => false;

        public void Attack()
        {
           
        }

        public void SetIsAttacking(bool isAttacking)
        {
            
        }
    }
}
