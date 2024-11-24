using System.Collections;
using UnityEngine;

namespace EB
{
    public enum AI_AttackType { Heavy = 0, Light = 1, Kick = 2 }; // Ensure enum matches Animator parameter values

    [System.Serializable]
    public class AI_Attack
    {
        public string name;
        public float duration; // Attack duration (time spent in the attack animation)
    }

    public class EnemyAttacks : MonoBehaviour
    {
        [Header("Attack Settings")]
        public AI_Attack heavyAttack;
        public AI_Attack lightAttack;
        public AI_Attack kickAttack;
        public Animator animator; // Reference to Animator

        [Header("Attack Timing")]
        public float minDelay = 0.5f; // Minimum time between attacks
        public float maxDelay = 1f; // Maximum time between attacks

        private bool isAttacking = false;

        private void Start()
        {
            animator = GetComponent<Animator>();

            StartCoroutine(RandomAttackRoutine());
        }

        private IEnumerator RandomAttackRoutine()
        {
            while (true)
            {
                if (!isAttacking)
                {
                    // Wait for a random delay before the next attack
                    float delay = Random.Range(minDelay, maxDelay);
                    yield return new WaitForSeconds(delay);

                    // Pick and perform a random attack
                    AI_AttackType attackType = (AI_AttackType)Random.Range(0, 3);
                    PerformAttack(attackType);
                }

                yield return null;
            }
        }

        private void PerformAttack(AI_AttackType attackType)
        {
            if (isAttacking) return; // Avoid starting a new attack if already attacking

            isAttacking = true;

            AI_Attack selectedAttack = null;

            // Set the selected attack based on the type
            switch (attackType)
            {
                case AI_AttackType.Heavy:
                    selectedAttack = heavyAttack;
                    break;
                case AI_AttackType.Light:
                    selectedAttack = lightAttack;
                    break;
                case AI_AttackType.Kick:
                    selectedAttack = kickAttack;
                    break;
            }

            if (selectedAttack != null)
            {
                Debug.Log($"AI performing attack: {selectedAttack.name} with type: {(int)attackType}");

                // Set the attack type and trigger the attack animation
                animator.SetInteger("AttackType", (int)attackType); // Set AttackType for the transition
                animator.SetBool("IsAttacking", true); // Start the attack transition
                animator.SetTrigger("AttackTrigger"); // Trigger the animation

                // Wait for 1.5 seconds before resetting everything, regardless of animation duration
                StartCoroutine(WaitForAttackToFinish(1.5f)); // Use a fixed 1.5-second wait
            }
            else
            {
                Debug.LogError("Selected attack is null!");
                isAttacking = false;
            }
        }

        private IEnumerator WaitForAttackToFinish(float waitTime)
        {
            // Wait for the specified time (1.5 seconds)
            yield return new WaitForSeconds(waitTime);

            // After the wait time finishes, reset all parameters
            ResetAttackState();
        }

        private void ResetAttackState()
        {
            // Reset animator parameters after the attack animation finishes
            animator.SetBool("IsAttacking", false); // Reset IsAttacking
            animator.SetInteger("AttackType", -1); // Reset AttackType (to ensure transitions)
            animator.ResetTrigger("AttackTrigger"); // Reset AttackTrigger

            // Optionally ensure the animator transitions back to Idle
            //AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            //if (!stateInfo.IsName("Idle"))
            //{
            //    animator.Play("Idle"); // Force transition to Idle state if not already there
            //}

            isAttacking = false; // Allow new attacks
            Debug.Log("AI attack reset complete.");
        }
    }
}
