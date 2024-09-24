using UnityEngine;

namespace HomeByMarch
{
    public class AttackState : BaseState
    {
        private static readonly int AttackHash = Animator.StringToHash("Attack");
        private float attackDuration;  // Duration of the attack (in seconds)
        private float elapsedTime = 0f;

        public AttackState(PlayerControllers player, Animator animator, float duration) : base(player, animator)
        {
            attackDuration = duration;  // Set the attack duration through constructor
        }

        public override void OnEnter()
        {
            Debug.Log("AttackState.OnEnter");
            // Start the attack animation
            // animator.CrossFade(AttackHash, crossFadeDuration);
            player.isAttacking = true;  // Set attacking state to true
            elapsedTime = 0f;  // Reset elapsed time when entering the state
        }

        public override void FixedUpdate()
        {
            elapsedTime += Time.fixedDeltaTime;

            // If the attack animation has played for the specified duration, exit the attack state
            if (elapsedTime >= attackDuration)
            {
                player.isAttacking = false;  // Reset attacking state in player
                // stateMachine.SetState(new LocomotionState(player, animator));  // Transition back to locomotion or idle
            }
        }
    }
}