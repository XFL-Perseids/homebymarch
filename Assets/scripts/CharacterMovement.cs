using UnityEngine;

namespace Cinemachine.Examples
{
    [AddComponentMenu("")] // Don't display in add component menu
    [RequireComponent(typeof(Rigidbody))]
    public class CharacterMovement2 : MonoBehaviour
    {
        public bool useCharacterForward = false;
        public bool lockToCameraForward = false;
        public float turnSpeed = 10f;
        public KeyCode sprintJoystick = KeyCode.JoystickButton2;
        public KeyCode sprintKeyboard = KeyCode.Space;

        public FixedJoystick joystick; // Add the joystick reference
        public float moveSpeed = 5f; // Movement speed
        private Rigidbody _rigidbody;
        private Animator anim;
        private Camera mainCamera;
        private Vector3 targetDirection;

        private bool isSprinting = false;
        private float turnSpeedMultiplier;
        private float speed;
        private Vector2 input;
        private float velocity;

        // Reference to WalkingSound component
        private WalkingSound walkingSound;

        // Walking sound cooldown variables
        public float soundCooldown = 0.5f; // Delay between steps in seconds
        private float soundTimer = 0f; // Tracks the time since the last step sound

        // Speed threshold for playing walking sound
        public float walkSpeedThreshold = 0.1f;

        // Use this for initialization
        void Start()
        {
            anim = GetComponent<Animator>();
            _rigidbody = GetComponent<Rigidbody>();
            mainCamera = Camera.main;

            // Get the WalkingSound component attached to the character
            walkingSound = GetComponent<WalkingSound>();
        }

        // Update is called once per frame
        void FixedUpdate()
        {
#if ENABLE_LEGACY_INPUT_MANAGER
            input.x = joystick.Horizontal;
            input.y = joystick.Vertical;

            // Set speed based on joystick input
            if (useCharacterForward)
                speed = Mathf.Abs(input.x) + input.y;
            else
                speed = Mathf.Abs(input.x) + Mathf.Abs(input.y);

            speed = Mathf.Clamp(speed, 0f, 1f);
            speed = Mathf.SmoothDamp(anim.GetFloat("Speed"), speed, ref velocity, 0.1f);
            anim.SetFloat("Speed", speed);

            // Only trigger walking sound if the character is moving above a small threshold
            if (speed > walkSpeedThreshold && walkingSound != null)
            {
                // Increment the sound timer
                soundTimer += Time.deltaTime;

                // Play sound only if enough time has passed since the last sound
                if (soundTimer >= soundCooldown)
                {
                    walkingSound.playSound();
                    soundTimer = 0f; // Reset the timer after playing the sound
                }
            }

            // Set sprinting
            isSprinting = ((Input.GetKey(sprintJoystick) || Input.GetKey(sprintKeyboard)) && input != Vector2.zero);
            anim.SetBool("isSprinting", isSprinting);

            // Update target direction relative to camera
            UpdateTargetDirection();

            // Move the player
            _rigidbody.velocity = new Vector3(input.x * moveSpeed, _rigidbody.velocity.y, input.y * moveSpeed);

            if (input != Vector2.zero && targetDirection.magnitude > 0.1f)
            {
                Vector3 lookDirection = targetDirection.normalized;
                Quaternion freeRotation = Quaternion.LookRotation(lookDirection, transform.up);
                var differenceRotation = freeRotation.eulerAngles.y - transform.eulerAngles.y;
                var eulerY = transform.eulerAngles.y;

                if (differenceRotation < 0 || differenceRotation > 0) eulerY = freeRotation.eulerAngles.y;
                var euler = new Vector3(0, eulerY, 0);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(euler), turnSpeed * turnSpeedMultiplier * Time.deltaTime);
            }
#else
            InputSystemHelper.EnableBackendsWarningMessage();
#endif
        }

        public virtual void UpdateTargetDirection()
        {
            if (!useCharacterForward)
            {
                turnSpeedMultiplier = 1f;
                var forward = mainCamera.transform.TransformDirection(Vector3.forward);
                forward.y = 0;

                // Get the right-facing direction of the referenceTransform
                var right = mainCamera.transform.TransformDirection(Vector3.right);

                // Determine the direction the player will face based on input
                targetDirection = input.x * right + input.y * forward;
            }
            else
            {
                turnSpeedMultiplier = 0.2f;
                var forward = transform.TransformDirection(Vector3.forward);
                forward.y = 0;

                // Get the right-facing direction of the referenceTransform
                var right = transform.TransformDirection(Vector3.right);
                targetDirection = input.x * right + Mathf.Abs(input.y) * forward;
            }
        }
    }
}
