

using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(Animator))]
    public class PlayerAnimation : MonoBehaviour
    {
        private readonly int isMoveAnimationHash = Animator.StringToHash("IsMove");
        private readonly int isAttackAnimationHash = Animator.StringToHash("IsAttack");

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        public void SetMoveAnimation(bool isMove)
        {
            _animator.SetBool(isMoveAnimationHash, isMove);
        }

        public void SetAttackAnimation()
        {
            _animator.SetTrigger(isAttackAnimationHash);
        }

    }
}
