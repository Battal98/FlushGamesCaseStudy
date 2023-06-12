using Enums;
using Keys;
using Managers;
using UnityEngine;

namespace Controllers
{
    public class PlayerAnimationController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        #endregion

        #region Serialized Variables

        [SerializeField] private PlayerManager playerManager;

        [SerializeField] private Animator animator;

        #endregion

        #region Private Variables

        private PlayerAnimationStates _currentAnimationState;

        #endregion

        #endregion
        private void Awake()
        {
            Init();
        }
        private void Init()
        {
            animator = GetComponent<Animator>();
        }

        public void PlayAnimation(PlayerAnimationStates animationStates)
        {
            if (animationStates == _currentAnimationState) return;
            animator.Play(animationStates.ToString());
            _currentAnimationState = animationStates;
        }
    }
}