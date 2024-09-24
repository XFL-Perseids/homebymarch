using UnityEngine;

namespace HomeByMarch {
    public class AttackState : BaseState {
        public AttackState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter() {
            animator.CrossFade(AttackHash, crossFadeDuration);
            Debug.Log("AttackState.OnEnter");
            //
            player.Attack();
        }

        public override void FixedUpdate() {
            // player.HandleMovement();
        }
    }
}