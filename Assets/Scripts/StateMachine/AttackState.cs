﻿using UnityEngine;

namespace HomeByMarch {
    public class AttackState : BaseState {
        public AttackState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter() {
            Debug.Log("AttackState.OnEnter");
            animator.CrossFade(AttackHash, crossFadeDuration);
            player.Attack();
        }

        public override void FixedUpdate() {
            player.HandleMovement();
        }
    }
}   