using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EB
{
    public class PlayerMovement : MonoBehaviour
    {

        public float horizontal;
        
        public float moveAmount;

        PlayerInput inputActions;

        Vector2 movementInput;

        public void OnEnable()
        {
            inputActions = new PlayerInput();
            inputActions.PlayerMovement.Movement.performed += inputActions => movementInput = inputActions.ReadValue<Vector2>();
            inputActions.Enable();
        }

        public void OnDisable()
        {
            inputActions.Disable();
        }


        public void TickInput(float delta)
        {
           MoveInput(delta);
        }

        public void MoveInput(float delta)
        {
            horizontal = movementInput.x;
            moveAmount = Mathf.Clamp01(Mathf.Abs(horizontal));
            
        }

    }
}

