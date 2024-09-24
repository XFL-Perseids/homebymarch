using System.Collections.Generic;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;
using Utilities;

namespace HomeByMarch {
    public class PlayerController : ValidatedMonoBehaviour {
        [Header("References")]
        // [SerializeField, Self] Rigidbody rb;
        // [SerializeField, Self] GroundChecker groundChecker;
        [SerializeField, Self] Animator animator;
        [SerializeField] CharacterController controller;
        [SerializeField, Anywhere] CinemachineVirtualCamera freeLookVCam;
        [SerializeField, Anywhere] InputReader input;
        
        [Header("Movement Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;
        
        [Header("Jump Settings")]
        [SerializeField] float jumpForce = 10f;
        [SerializeField] float jumpDuration = 0.5f;
        [SerializeField] float jumpCooldown = 0f;
        [SerializeField] float gravityMultiplier = 3f;
        
        [Header("Dash Settings")]
        [SerializeField] float dashForce = 10f;
        [SerializeField] float dashDuration = 1f;
        [SerializeField] float dashCooldown = 2f;
        
        [Header("Attack Settings")]
        [SerializeField] float attackCooldown = 0.5f;
        [SerializeField] float attackDistance = 1f;
        [SerializeField] int attackDamage = 10;

        const float ZeroF = 0f;
        
        Transform mainCam;
        
        float currentSpeed;
        float velocity;
        float jumpVelocity;
        float dashVelocity = 1f;

        Vector3 movement;

        List<Timer> timers;
        CountdownTimer jumpTimer;
        CountdownTimer jumpCooldownTimer;
        CountdownTimer dashTimer;
        CountdownTimer dashCooldownTimer;
        CountdownTimer attackTimer;
        
        StateMachine stateMachine;
        
        // Animator parameters
        static readonly int Speed = Animator.StringToHash("Speed");

        void Awake() {
            mainCam = Camera.main.transform;
            freeLookVCam.Follow = transform;
            freeLookVCam.LookAt = transform;
            // Invoke event when observed transform is teleported, adjusting freeLookVCam's position accordingly
            freeLookVCam.OnTargetObjectWarped(transform, transform.position - freeLookVCam.transform.position - Vector3.forward);
            
            // rb.freezeRotation = true;
            
            SetupTimers();
            SetupStateMachine();
        }

        void SetupStateMachine() {
            // State Machine
            stateMachine = new StateMachine();

            // Declare states
            var locomotionState = new LocomotionState(this, animator);
            // var jumpState = new JumpState(this, animator);
            // var dashState = new DashState(this, animator);
            var attackState = new AttackState(this, animator);

            // Define transitions
            // At(locomotionState, jumpState, new FuncPredicate(() => jumpTimer.IsRunning));
            // At(locomotionState, dashState, new FuncPredicate(() => dashTimer.IsRunning));
            At(locomotionState, attackState, new FuncPredicate(() => attackTimer.IsRunning));
            At(attackState, locomotionState, new FuncPredicate(() => !attackTimer.IsRunning));
            Any(locomotionState, new FuncPredicate(ReturnToLocomotionState));

            // Set initial state
            stateMachine.SetState(locomotionState);
        }

        bool ReturnToLocomotionState() {
            return !attackTimer.IsRunning 
                   && !jumpTimer.IsRunning 
                   && !dashTimer.IsRunning;
        }

        void SetupTimers() {
            // Setup timers
            jumpTimer = new CountdownTimer(jumpDuration);
            jumpCooldownTimer = new CountdownTimer(jumpCooldown);

            jumpTimer.OnTimerStart += () => jumpVelocity = jumpForce;
            jumpTimer.OnTimerStop += () => jumpCooldownTimer.Start();

            dashTimer = new CountdownTimer(dashDuration);
            dashCooldownTimer = new CountdownTimer(dashCooldown);

            dashTimer.OnTimerStart += () => dashVelocity = dashForce;
            dashTimer.OnTimerStop += () => {
                dashVelocity = 1f;
                dashCooldownTimer.Start();
            };

            attackTimer = new CountdownTimer(attackCooldown);

            timers = new(5) {jumpTimer, jumpCooldownTimer, dashTimer, dashCooldownTimer, attackTimer};
        }

        void At(IState from, IState to, IPredicate condition) => stateMachine.AddTransition(from, to, condition);
        void Any(IState to, IPredicate condition) => stateMachine.AddAnyTransition(to, condition);

        void Start() => input.EnablePlayerActions();

        void OnEnable() {
            
            input.Dash += OnDash;
            input.Attack += OnAttack;
        }
        
        void OnDisable() {

            input.Attack -= OnAttack;
        }
        
        void OnAttack() {
            if (!attackTimer.IsRunning) {
                attackTimer.Start();
            }
        }

        public void Attack() {
            Vector3 attackPos = transform.position + transform.forward;
            Collider[] hitEnemies = Physics.OverlapSphere(attackPos, attackDistance);
            
            // foreach (var enemy in hitEnemies) {
            //     Debug.Log(enemy.name);
            //     if (enemy.CompareTag("Enemy")) {
            //         enemy.GetComponent<Health>().TakeDamage(attackDamage);
            //     }
            // }
        }

        // void OnJump(bool performed) {
        //     if (performed && !jumpTimer.IsRunning && !jumpCooldownTimer.IsRunning && groundChecker.IsGrounded) {
        //         jumpTimer.Start();
        //     } else if (!performed && jumpTimer.IsRunning) {
        //         jumpTimer.Stop();
        //     }
        // }
        
        void OnDash(bool performed) {
            if (performed && !dashTimer.IsRunning && !dashCooldownTimer.IsRunning) {
                dashTimer.Start();
            } else if (!performed && dashTimer.IsRunning) {
                dashTimer.Stop();
            }
        }

        void Update() {
            movement = new Vector3(input.Direction.x, 0f, input.Direction.y);
            stateMachine.Update();

            HandleTimers();
            UpdateAnimator();
        }

        void FixedUpdate() {
            stateMachine.FixedUpdate();
        }

        void UpdateAnimator() {
            animator.SetFloat(Speed, currentSpeed);
        }

        void HandleTimers() {
            foreach (var timer in timers) {
                timer.Tick(Time.deltaTime);
            }
        }

        // public void HandleJump() {
        //     // If not jumping and grounded, keep jump velocity at 0
        //     if (!jumpTimer.IsRunning && groundChecker.IsGrounded) {
        //         jumpVelocity = ZeroF;
        //         return;
        //     }
            
        //     if (!jumpTimer.IsRunning) {
        //         // Gravity takes over
        //         jumpVelocity += Physics.gravity.y * gravityMultiplier * Time.fixedDeltaTime;
        //     }
            
        //     // Apply velocity
        //     rb.velocity = new Vector3(rb.velocity.x, jumpVelocity, rb.velocity.z);
        // }

        public void HandleMovement()
{
    // Get the input direction
    var movementInput = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;

    // If there is any movement input
    if (movementInput.magnitude > ZeroF)
    {
        // Adjust movement direction relative to camera
        Vector3 cameraForward = mainCam.forward;
        Vector3 cameraRight = mainCam.right;

        // Flatten the camera's forward and right vectors to ignore vertical direction
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        // Normalize the camera vectors
        cameraForward.Normalize();
        cameraRight.Normalize();

        // Calculate movement direction relative to the camera
        Vector3 movementDirection = (cameraForward * movementInput.z + cameraRight * movementInput.x).normalized;

        // Handle rotation based on movement direction
        HandleRotation(movementDirection);

        // Handle movement based on the calculated movement direction
        HandleCharacterController(movementDirection);

        // Smooth the speed based on input magnitude
        SmoothSpeed(movementInput.magnitude);
    }
    else
    {
        // If no input, smooth to zero speed
        SmoothSpeed(ZeroF);
    }
}


        void HandleCharacterController(Vector3 adjustedDirection)
        {
            // Move the player
            var adjustedMovement = adjustedDirection * (moveSpeed * Time.deltaTime);
            controller.Move(adjustedMovement);
        }

        void HandleRotation(Vector3 adjustedDirection)
        {
            // Adjust rotation to match movement direction
            var targetRotation = Quaternion.LookRotation(adjustedDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            transform.LookAt(transform.position + adjustedDirection);
        }


        void SmoothSpeed(float value)
        {
            currentSpeed = Mathf.SmoothDamp(currentSpeed, value, ref velocity, smoothTime);
        }
    }
}