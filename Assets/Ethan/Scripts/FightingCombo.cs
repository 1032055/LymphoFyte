using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace EB
{
    public enum AttackType { heavy = 0, light = 1, kick = 2 };

    public class FightingCombo : MonoBehaviour
    {
        // Assign attacks through the Inspector
        [Header("Attacks")]
        public Attack heavyAttack;
        public Attack lightAttack;
        public Attack kickAttack;
        public List<Combo> combos;
        public float comboLeeway = 0.25f;  // Combo leeway duration

        // Animator for attacks
        [Header("Components")]
        public Animator animator;

        // Internal states
        public Attack curAttack = null;
        public ComboInput lastInput = null;
        public List<int> currentCombos = new List<int>();
        public float timer = 0;
        public float leeway = 0;  // Leeway timer for combo input window
        public bool skip = false;

        // Input actions from the Input System
        private PlayerInput playerInput;
        private InputAction heavyAction;
        private InputAction lightAction;
        private InputAction kickAction;

        private void Awake()
        {
            playerInput = new PlayerInput();
            heavyAction = playerInput.PlayerAttacks.HeavyAttack;
            lightAction = playerInput.PlayerAttacks.LightAttack;
            kickAction = playerInput.PlayerAttacks.KickAttack;

            heavyAction.performed += ctx => OnAttackInput(new ComboInput(AttackType.heavy));
            lightAction.performed += ctx => OnAttackInput(new ComboInput(AttackType.light));
            kickAction.performed += ctx => OnAttackInput(new ComboInput(AttackType.kick));
        }

        private void OnEnable()
        {
            playerInput.Enable();
        }

        private void OnDisable()
        {
            playerInput.Disable();
        }

        private void Start()
        {
            if (animator == null)
            {
                Debug.LogError("Animator component not found!");
                return;
            }

            if (heavyAttack == null || lightAttack == null || kickAttack == null)
            {
                Debug.LogError("One or more attacks are not assigned!");
                return;
            }

            if (combos == null)
            {
                combos = new List<Combo>();  // Initialize combos if it's null
            }

            animator = GetComponent<Animator>();
            PrimeCombos();
        }

        void Update()
        {
            // If there's an active attack, update the timer
            if (curAttack != null)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    curAttack = null; // Reset if the attack is finished
                }
                return;
            }

            // If there are active combos, handle the leeway timer
            if (currentCombos.Count > 0)
            {
                leeway += Time.deltaTime; // Increment leeway timer as long as combos are active

                // If the leeway time exceeds the allowed limit, complete the current combo and reset
                if (leeway >= comboLeeway)
                {
                    // If a valid combo was entered, perform the attack
                    if (lastInput != null)
                    {
                        Attack(getAttackFromType(lastInput.type)); // Execute last valid attack
                        lastInput = null; // Reset input after performing attack
                    }
                    ResetCombos(); // Reset active combos
                }
            }
        }


        void PrimeCombos()
        {
            if (combos == null || combos.Count == 0)
            {
                return; // Prevent null reference if combos list is empty or null
            }

            for (int i = 0; i < combos.Count; i++)
            {
                Combo c = combos[i];

                if (c.onInputted == null)
                {
                    c.onInputted = new UnityEvent();  // Ensure onInputted is not null
                }

                c.onInputted.AddListener(() =>
                {
                    // Attack function logic
                    Debug.Log("Combo Completed: " + c.name);
                    skip = true;
                    Attack(c.comboAttack);
                    ResetCombos();
                });
            }
        }


        void OnAttackInput(ComboInput input)
        {
            if (input == null)
            {
                Debug.LogError("Input is null!");
                return;
            }

            // Only proceed if no current attack is in progress
            if (curAttack != null && curAttack.name != "none")
            {
                return; // Ignore further inputs until the current attack is done
            }

            lastInput = input;

            // Increment leeway timer on each attack input
            leeway += Time.deltaTime; // Accumulate leeway timer every time an attack is inputted

            // Try to continue any active combo
            List<int> remove = new List<int>();

            // Debug: Track combo progress
            Debug.Log("Trying to continue combos...");

            for (int i = 0; i < currentCombos.Count; i++)
            {
                Combo c = combos[currentCombos[i]];
                if (c.continueCombo(input)) // Check if the combo continues with the input
                {
                    leeway = 0; // Reset leeway if the combo continues
                    Debug.Log("Combo Continued: " + c.name); // Debug combo continuation
                }
                else
                {
                    remove.Add(i); // Mark combo for removal if it doesn't continue
                    Debug.Log("Combo Reset: " + c.name); // Debug combo reset
                }
            }

            // If no combo continued, check if a new combo starts
            bool comboStarted = false;
            for (int i = 0; i < combos.Count; i++)
            {
                if (currentCombos.Contains(i))
                    continue;

                if (combos[i].continueCombo(input))  // Check if this combo is valid to start
                {
                    currentCombos.Add(i);  // Add the new combo to the active list
                    leeway = 0;  // Reset leeway time for the new combo
                    comboStarted = true;
                    Debug.Log("New Combo Started: " + combos[i].name); // Debug new combo start
                    break;
                }
            }

            // If no combo was started, directly trigger the attack
            if (!comboStarted)
            {
                Attack(getAttackFromType(input.type));
            }

            // Remove any invalid combos
            foreach (int i in remove)
            {
                currentCombos.RemoveAt(i);
            }

            // If no combos are active anymore, reset them
            if (currentCombos.Count <= 0)
            {
                ResetCombos();
            }
        }





        void ResetCombos()
        {
            leeway = 0;  // Reset leeway
            for (int i = 0; i < currentCombos.Count; i++)
            {
                Combo c = combos[currentCombos[i]];
                c.ResetCombo();
            }
            currentCombos.Clear();
            Debug.Log("Combos reset.");
        }

        void Attack(Attack attack)
        {
            curAttack = attack;
            timer = attack.length;
            animator.Play(attack.name, -1, 0);
            Debug.Log($"Attack triggered: {attack.name}");
            // Set the trigger and IsAttacking to true
            animator.ResetTrigger("AttackTrigger"); // Reset first to clear any previous trigger
            animator.SetTrigger("AttackTrigger");   // Trigger the current attack
            animator.SetBool("IsAttacking", true);  // Set IsAttacking to true

            // Start coroutine to reset IsAttacking after attack duration
            StartCoroutine(ResetToIdleAfterAttack(attack.length));
        }



        IEnumerator ResetToIdleAfterAttack(float attackDuration)
        {
            yield return new WaitForSeconds(attackDuration);

            // Reset IsAttacking and the AttackTrigger
            animator.SetBool("IsAttacking", false);  // Transition back to idle
            animator.ResetTrigger("AttackTrigger");  // Ensure trigger is reset
        }


        Attack getAttackFromType(AttackType t)
        {
            // Your logic to return the correct attack based on the type
            if (t == AttackType.heavy)
            {
                return heavyAttack;
            }
            else if (t == AttackType.light)
            {
                return lightAttack;
            }
            else if (t == AttackType.kick)
            {
                return kickAttack;
            }

            Debug.LogError("Invalid attack type: " + t);
            return null;
        }
    }

    // Attack and Combo classes (unchanged)
    [System.Serializable]
    public class Attack { public string name; public float length; }

    [System.Serializable]
    public class ComboInput
    {
        public AttackType type;

        public ComboInput(AttackType t)
        {
            type = t;
        }

        // Define the isSameAS method to compare the type of this ComboInput with another
        public bool isSameAS(ComboInput test)
        {
            return type == test.type;
        }
    }

    [System.Serializable]
    public class Combo
    {
        public string name;
        public List<ComboInput> inputs;
        public Attack comboAttack;
        public UnityEvent onInputted = new UnityEvent();
        int curInput = 0;

        // Method to check if the combo can continue with the new input
        public bool continueCombo(ComboInput input)
        {
            if (inputs[curInput].isSameAS(input))
            {
                curInput++;
                if (curInput >= inputs.Count)
                {
                    onInputted.Invoke();  // Combo completed
                    curInput = 0;  // Reset input index
                    return true;
                }
                return true;
            }
            else
            {
                curInput = 0;  // Reset combo if input doesn't match
                return false;
            }
        }

        // Reset the combo to its initial state
        public void ResetCombo()
        {
            curInput = 0;
        }
    }
}
