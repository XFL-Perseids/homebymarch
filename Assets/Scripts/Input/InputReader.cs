using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Events;
using static PlayerInputActions;


namespace HomeByMarch
{
    [CreateAssetMenu(fileName = "InputReader", menuName="HomeByMarch/InputReader")]

    public class InputReader : ScriptableObject, IPlayerActions
    {

        public event UnityAction<Vector2> Move = delegate { };
        public event UnityAction<Vector2, bool> Look = delegate { };

        PlayerInputActions inputActions;
        public Vector3 Direction => inputActions.Player.Move.ReadValue<Vector2>();

        public void OnEnable()
        {
            if (inputActions == null)
            {
                inputActions = new PlayerInputActions();
                inputActions.Enable();
                inputActions.Player.SetCallbacks(this);

            }
            inputActions.Enable();
        }
        public void OnMove(InputAction.CallbackContext context)
        {

        }
        public void OnLook(InputAction.CallbackContext context)
        {
            //noop
        }
        public void OnFire(InputAction.CallbackContext context)
        {
            //noop
        }
        public void OnMouseControlCamera(InputAction.CallbackContext context)
        {
            //noop
        }
        public void OnRun(InputAction.CallbackContext context)
        {
            //noop
        }
        public void OnJump(InputAction.CallbackContext context)
        {
            //noop
        }
        public void OnNewaction(InputAction.CallbackContext context)
        {
            //noop
        }
    }

}