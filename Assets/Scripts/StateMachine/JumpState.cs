using UnityEngine;

namespace HomeByMarch {
    public class JumpState : BaseState {
        public JumpState(PlayerControllers player, Animator animator) : base(player, animator) { }

        public override void OnEnter() {
            animator.CrossFade(JumpHash, crossFadeDuration);
        }

        public override void FixedUpdate() {
            // player.HandleJump();
            // player.HandleMovement();
        }
    }
    
    namespace Platformer { }
}