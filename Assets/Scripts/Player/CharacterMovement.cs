using System.Collections.Generic;
using UnityEngine;

namespace HomeByMarch {
    [AddComponentMenu("")] // Don't display in add component menu
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour {
        [Header("Input Settings")]
        [SerializeField] FixedJoystick joystick;
        [SerializeField] KeyCode sprintJoystick = KeyCode.JoystickButton2;
        [SerializeField] KeyCode sprintKeyboard = KeyCode.Space;

        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 5f;
        [SerializeField] float turnSpeed = 10f;
        [SerializeField] bool useCharacterForward = false;
        [Header("Sound Settings")]
        [SerializeField] WalkingSound walkingSound;
        [SerializeField] float soundCooldown = 0.5f; // Delay between steps
        [SerializeField] float walkSpeedThreshold = 0.1f; // Speed threshold for walking sound

        // Private fields
        Rigidbody rb;
        Animator animator;
        Camera mainCamera;
        Vector3 targetDirection;

        bool isSprinting = false;
        float speed;
        float turnSpeedMultiplier;
        float velocity;
        float soundTimer = 0f; // Timer to track walking sound

        Vector2 input;
        StateMachine stateMachine;
        void Awake() {
    rb = GetComponent<Rigidbody>();
    animator = GetComponent<Animator>();
    mainCamera = Camera.main;

    if (walkingSound == null) {
        walkingSound = GetComponent<WalkingSound>();
    }

    if (rb == null || animator == null || mainCamera == null) {
        Debug.LogError("One of the required components is not assigned or missing!");
    }

    // State machine and state initialization
    stateMachine = new StateMachine();
    var locomotionState = new LocomotionState(this, animator);
    var sprintState = new JumpState(this, animator);
    
    At(locomotionState, sprintState, new FuncPredicate(() => isSprinting));
    At(sprintState, locomotionState, new FuncPredicate(() => !isSprinting));
    
    stateMachine.SetState(locomotionState);
}


        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from,to,condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to,condition);

        void Start() {
            // Enable input actions (if any)
        }

        void FixedUpdate() {
#if ENABLE_LEGACY_INPUT_MANAGER
            // Capture joystick input
            input.x = joystick.Horizontal;
            input.y = joystick.Vertical;

            // Handle movement logic
            stateMachine.FixedUpdate();

            // Handle walking sound
            HandleWalkingSound();

            // Handle sprinting
            isSprinting = (Input.GetKey(sprintJoystick) || Input.GetKey(sprintKeyboard)) && input != Vector2.zero;
            animator.SetBool("isSprinting", isSprinting);
#else
            InputSystemHelper.EnableBackendsWarningMessage();
#endif
        }

        public void HandleMovement() {
            // Calculate speed based on input
            if (useCharacterForward) {
                speed = Mathf.Abs(input.x) + input.y;
            } else {
                speed = Mathf.Abs(input.x) + Mathf.Abs(input.y);
            }

            speed = Mathf.Clamp(speed, 0f, 1f);
            speed = Mathf.SmoothDamp(animator.GetFloat("Speed"), speed, ref velocity, 0.1f);
            animator.SetFloat("Speed", speed);

            // Update target direction based on camera
            UpdateTargetDirection();

            // Move the player
            Vector3 moveDirection = new Vector3(input.x * moveSpeed, rb.velocity.y, input.y * moveSpeed);
            rb.velocity = moveDirection;

            // Rotate the player
            if (input != Vector2.zero && targetDirection.magnitude > 0.1f) {
                HandleRotation();
            }
        }

        void HandleWalkingSound() {
            // Play walking sound if moving
            if (speed > walkSpeedThreshold && walkingSound != null) {
                soundTimer += Time.deltaTime;
                if (soundTimer >= soundCooldown) {
                    walkingSound.playSound();
                    soundTimer = 0f; // Reset timer
                }
            }
        }

        void HandleRotation() {
            Vector3 lookDirection = targetDirection.normalized;
            Quaternion freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
            float differenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
            float eulerY = transform.eulerAngles.y;

            if (differenceRotation != 0) {
                eulerY = freeRotation.eulerAngles.y;
            }

            Vector3 euler = new Vector3(0, eulerY, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * turnSpeedMultiplier * Time.deltaTime);
        }

        public void UpdateTargetDirection() {
            if (!useCharacterForward) {
                turnSpeedMultiplier = 1f;
                Vector3 forward = mainCamera.transform.TransformDirection(Vector3.forward);
                forward.y = 0;
                Vector3 right = mainCamera.transform.TransformDirection(Vector3.right);
                targetDirection = input.x * right + input.y * forward;
            } else {
                turnSpeedMultiplier = 0.2f;
                Vector3 forward = transform.TransformDirection(Vector3.forward);
                forward.y = 0;
                Vector3 right = transform.TransformDirection(Vector3.right);
                targetDirection = input.x * right + Mathf.Abs(input.y) * forward;
            }
        }
    }
}
