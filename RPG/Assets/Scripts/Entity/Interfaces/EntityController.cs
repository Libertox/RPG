
using UnityEngine;

namespace Entity
{
    public class EntityController : MonoBehaviour
    {
        public IAnimationController AnimationController => _animationController;
        public ICombatController CombatController => _combatController;

        protected IAnimationController _animationController;
        protected ICombatController _combatController;

        public virtual void Destroy()
        {

        }

    }
}
