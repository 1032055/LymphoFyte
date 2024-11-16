using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EB
{
    public class AnimationHandler : MonoBehaviour
    {
        public Animator animator;
        int horizontal;

        public void Initialize()
        {
            animator = GetComponent<Animator>();
            horizontal = Animator.StringToHash("Horizontal");
        }

        public void UpdateAnimatorValues(float horizontalMovement, float verticalMovement)
        {
            float h = 0;

            if (horizontalMovement > 0 && horizontalMovement < 0.55f)
            {
                h = 0.5f;
            }
            else if (horizontalMovement > 0.55f)
            {
                h = 1;
            }
            else if (horizontalMovement < 0 && horizontalMovement > -0.55f)
            {
                h = -0.5f;
            }
            else if (horizontalMovement < -0.55f)
            {
                h = -1;
            }
            else
            {
                h = 0;
            }

            animator.SetFloat(horizontal, h, 0.1f, Time.deltaTime);
        }
    }
}

