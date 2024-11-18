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
        [Header("Attacks")]
        public Attack heavyAttack;
        public Attack lightAttack;
        public Attack kickAttack;
        public List<Combo> combos;
        public float comboLeeway = 0.25f;  

        // Animator for attacks
        [Header("Components")]
        public Animator animator;

        // Internal states
        public Attack curAttack = null;
        public ComboInput lastInput = null;
        public List<int> currentCombos = new List<int>();
        public float timer = 0;
        public float leeway = 0;  
        public bool skip = false;

        // Input actions from the Input System
        private PlayerInput playerInput;
        private InputAction heavyAction;
        private InputAction lightAction;
        private InputAction kickAction;

        private PlayerLocomotion playerLocomotion;

        public bool isStunned = false;

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
                combos = new List<Combo>();  
            }

            playerLocomotion = GetComponentInParent<PlayerLocomotion>();

            animator = GetComponent<Animator>();
            PrimeCombos();
        }

        public void Update()
        {
            if (curAttack != null)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else
                {
                    curAttack = null; 
                }
                return;
            }

            if (currentCombos.Count > 0)
            {
                leeway += Time.deltaTime; 

                if (leeway >= comboLeeway)
                {
                    if (lastInput != null)
                    {
                        Attack(getAttackFromType(lastInput.type)); 
                        lastInput = null;
                    }
                    ResetCombos(); 
                }
            }
        }


        public void PrimeCombos()
        {
            if (combos == null || combos.Count == 0)
            {
                return; 
            }

            for (int i = 0; i < combos.Count; i++)
            {
                Combo c = combos[i];

                if (c.onInputted == null)
                {
                    c.onInputted = new UnityEvent();  
                }

                c.onInputted.AddListener(() =>
                {
                    Debug.Log("Combo Completed: " + c.name);
                    skip = true;
                    Attack(c.comboAttack);
                    ResetCombos();
                });
            }
        }


        public void OnAttackInput(ComboInput input)
        {
            if (isStunned)
            {
                return;
            }

            if (input == null)
            {
                Debug.LogError("Input is null!");
                return;
            }

            if (curAttack != null && curAttack.name != "none")
            {
                return; 
            }

            lastInput = input;

            leeway += Time.deltaTime; 


            List<int> remove = new List<int>();

            Debug.Log("Trying to continue combos...");

            for (int i = 0; i < currentCombos.Count; i++)
            {
                Combo c = combos[currentCombos[i]];
                if (c.continueCombo(input)) 
                {
                    leeway = 0; 
                    Debug.Log("Combo Continued: " + c.name); 
                }
                else
                {
                    remove.Add(i);
                    Debug.Log("Combo Reset: " + c.name);
                }
            }

            bool comboStarted = false;
            for (int i = 0; i < combos.Count; i++)
            {
                if (currentCombos.Contains(i))
                    continue;

                if (combos[i].continueCombo(input))  
                {
                    currentCombos.Add(i);  
                    leeway = 0; 
                    comboStarted = true;
                    Debug.Log("New Combo Started: " + combos[i].name); 
                    break;
                }
            }

            if (!comboStarted)
            {
                Attack(getAttackFromType(input.type));
            }

            foreach (int i in remove)
            {
                currentCombos.RemoveAt(i);
            }

            if (currentCombos.Count <= 0)
            {
                ResetCombos();
            }
        }





        public void ResetCombos()
        {
            leeway = 0;  
            for (int i = 0; i < currentCombos.Count; i++)
            {
                Combo c = combos[currentCombos[i]];
                c.ResetCombo();
            }
            currentCombos.Clear();
            Debug.Log("Combos reset.");
        }

        public void Attack(Attack attack)
        {
            if (isStunned)
            {
                return;
            }

            curAttack = attack;
            timer = attack.length;
            animator.Play(attack.name, -1, 0);
            Debug.Log($"Attack triggered: {attack.name}");
            animator.ResetTrigger("AttackTrigger"); 
            animator.SetTrigger("AttackTrigger");  
            animator.SetBool("IsAttacking", true);  

            StartCoroutine(ResetToIdleAfterAttack(attack.length));

            if (playerLocomotion != null)
            {
                playerLocomotion.StartAttackMovement(attack.length);
            }
        }

        

        public IEnumerator ResetToIdleAfterAttack(float attackDuration)
        {
            yield return new WaitForSeconds(attackDuration);

            animator.SetBool("IsAttacking", false);  
            animator.ResetTrigger("AttackTrigger");  
        }


        public Attack getAttackFromType(AttackType t)
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
