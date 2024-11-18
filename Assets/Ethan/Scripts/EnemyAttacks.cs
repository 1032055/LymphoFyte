using EB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttacks : MonoBehaviour
{
    public FightingCombo fightingCombo; // Reference to the FightingCombo script
    public float minDelay = 1f;         // Minimum delay between attacks
    public float maxDelay = 3f;         // Maximum delay between attacks

    private bool isAttacking = false;

    private void Start()
    {
        if (fightingCombo == null)
        {
            Debug.LogError("FightingCombo script is not assigned to the AIAttacker!");
            return;
        }

        StartCoroutine(RandomAttackRoutine());
    }

    private IEnumerator RandomAttackRoutine()
    {
        while (true)
        {
            if (!isAttacking)
            {
                // Random delay before next attack
                float delay = Random.Range(minDelay, maxDelay);
                yield return new WaitForSeconds(delay);

                // Randomly pick an attack
                int attackIndex = Random.Range(0, 3); // 0 for heavy, 1 for light, 2 for kick
                AttackType attackType = (AttackType)attackIndex;

                // Execute the attack
                PerformAttack(attackType);
            }

            yield return null;
        }
    }

    private void PerformAttack(AttackType attackType)
    {
        if (fightingCombo == null || fightingCombo.isStunned)
            return;

        switch (attackType)
        {
            case AttackType.heavy:
                fightingCombo.Attack(fightingCombo.heavyAttack);
                break;
            case AttackType.light:
                fightingCombo.Attack(fightingCombo.lightAttack);
                break;
            case AttackType.kick:
                fightingCombo.Attack(fightingCombo.kickAttack);
                break;
        }

        isAttacking = true;

        // Reset the attacking state after the attack duration
        StartCoroutine(ResetAttackState(fightingCombo.curAttack?.length ?? 0f));
    }

    private IEnumerator ResetAttackState(float attackDuration)
    {
        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }
}
