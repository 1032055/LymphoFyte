using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EB
{
    public class PlayerLocomotion : MonoBehaviour
    {

        PlayerMovement movement;
        

        public Transform myTransform;
        public AnimationHandler animationHandler;

        public Rigidbody rb;

        [Header("Stats")]
        public float movementSpeed = 5;
        public float rotationSpeed = 10;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
            movement = GetComponent<PlayerMovement>();
            animationHandler = GetComponentInChildren<AnimationHandler>();
            myTransform = transform;
            animationHandler.Initialize();
        }

        // Update is called once per frame
        public void FixedUpdate()
        {
            float delta = Time.deltaTime;

            movement.TickInput(delta);

            // Assuming movement.horizontal is the left/right input as a float
            float horizontalInput = movement.horizontal;

            // Create the move direction vector for left/right movement only
            Vector3 moveDirection = new Vector3(horizontalInput, 0, 0) * movementSpeed;

            // Apply the movement to the Rigidbody
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, rb.velocity.z);

            animationHandler.UpdateAnimatorValues(horizontalInput, 0);
        }
    }
}

