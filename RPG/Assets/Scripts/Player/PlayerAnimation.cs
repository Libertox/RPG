

using UnityEngine;

namespace Player
{
    public class PlayerAnimation : MonoBehaviour
    {
        private readonly int isMoveAnimationHash = Animator.StringToHash("IsMove");

        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        public void SetMoveAnimation(bool isMove)
        {
            animator.SetBool(isMoveAnimationHash, isMove);
        }

    }
}
