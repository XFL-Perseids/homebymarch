using System.Collections.Generic;
using Cinemachine;
using KBCore.Refs;
using UnityEngine;


namespace HomeByMarch
{

    public class PlayerController : ValidatedMonoBehaviour
    {
        [Header("References")]
        [SerializeField] CharacterController controller;
        [SerializeField] Animator animator;
        [SerializeField] CinemachineVirtualCamera virtualCamera;
        [SerializeField] InputReader input;

        [Header("Settings")]
        [SerializeField] float moveSpeed = 6f;
        [SerializeField] float rotationSpeed = 15f;
        [SerializeField] float smoothTime = 0.2f;

        const float ZeroF = 0f;
        Transform mainCam;
        float currentSpeed;
        float velocity;

        void Awake()
        {
            mainCam = Camera.main.transform;
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;

        }

        void Update()
        {
            HandleMovement();
        }
        void HandleMovement()
        {
            // Use input directly without adjusting for camera rotation
            var movementDirection = new Vector3(input.Direction.x, 0f, input.Direction.y).normalized;

            if (movementDirection.magnitude > ZeroF)
            {
                HandleRotation(movementDirection); // Rotate the character based on the movement direction
                HandleCharacterController(movementDirection); // Move the character based on input direction
                // SmoothSpeed(movementDirection.magnitude); // Smooth speed based on input magnitude
            }
            else
            {
                SmoothSpeed(ZeroF); // If no input, smooth to zero speed
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