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
        public string handName = "Right";

        private void Awake()
        {
            damageCollider = GetComponent<Collider>();
            damageCollider.isTrigger = true;
            damageCollider.enabled = false;
        }

        public void EnableDamageCollider()
        {
            damageCollider.enabled = true;
        }

        public void DisableDamageCollider()
        {
            damageCollider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Hittable"))
            {
                PlayerBase playerBase = other.GetComponentInParent<PlayerBase>();
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

