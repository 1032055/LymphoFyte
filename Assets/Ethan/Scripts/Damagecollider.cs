using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EB
{
    public class Damagecollider : MonoBehaviour
    {
        private Collider damageCollider;

        [Header("Damage Settings")]
        public float currentDamage = 25f;

        [Header("Hand Identifier")]
        public string handName = "Right"; // Set to "Left" or "Right"

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.isTrigger = true;
            damageCollider.enabled = false; // Initially disabled
        }

        public void EnableDamageCollider()
        {
            Debug.Log($"{handName} hand collider enabled.");
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            Debug.Log($"{handName} hand collider disabled.");
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hittable"))
            {
                PlayerBase playerBase = other.GetComponent<PlayerBase>();
                playerBase?.TakeDamage(currentDamage);
            }

            if (other.CompareTag("Enemy"))
            {
                EnemyBase enemyBase = other.GetComponent<EnemyBase>();
                enemyBase?.TakeDamage(currentDamage);
            }
        }
    }
}

