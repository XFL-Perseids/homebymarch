using UnityEngine;

namespace HomeByMarch {
    public class LocomotionState : BaseState {
        public LocomotionState(PlayerControllers player, Animator animator) : base(player, animator) { }
        
        public override void OnEnter() {
            Debug.Log("LocomotionState.OnEnter");
            animator.CrossFade(LocomotionHash, crossFadeDuration);
        }
        
        public override void FixedUpdate() {
            player.HandleMovement();
        }
    }
}