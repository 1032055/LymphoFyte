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
        public float originalMovementSpeed;
        public float attackSlowMultiplier = 0.1f;

        FightingCombo fightingCombo;
        SFXHandler sfxHandler;

        public bool isStunned = false;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            movement = GetComponent<PlayerMovement>();
            animationHandler = GetComponentInChildren<AnimationHandler>();
            myTransform = transform;
            animationHandler.Initialize();

            originalMovementSpeed = movementSpeed;

            fightingCombo = GetComponentInChildren<FightingCombo>();

            sfxHandler = GetComponent<SFXHandler>();  
        }

        // Update is called once per frame
        public void FixedUpdate()
        {
            if (isStunned)
            {
                rb.velocity = new Vector3(0, rb.velocity.y, rb.velocity.z);
                return;
            }

            float delta = Time.deltaTime;

            movement.TickInput(delta);
            float horizontalInput = movement.horizontal;
            Vector3 moveDirection = new Vector3(horizontalInput, 0, 0) * movementSpeed;
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, rb.velocity.z);

            animationHandler.UpdateAnimatorValues(horizontalInput, 0);
        }

        public void StartAttackMovement(float delay)
        {
            movementSpeed *= attackSlowMultiplier;

            StartCoroutine(EndAttackAfterDelay(delay));

            sfxHandler.audioSource.clip = sfxHandler.punch;
            sfxHandler.audioSource.Play();
            
        }


        public IEnumerator EndAttackAfterDelay(float delay)
        {
            yield return new WaitForSeconds(1);
            EndAttackMovement();
            
        }
        public void EndAttackMovement()
        {
            movementSpeed = originalMovementSpeed;
        }
    }
}

