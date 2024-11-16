using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EB
{
    public class Damagecollider : MonoBehaviour
    {
        Collider damageCollider;

        public float currentDamage = 25;


        //private void Awake()
        //{
        //    damageCollider = GetComponent<Collider>();
        //    damageCollider.gameObject.SetActive(true);
        //    damageCollider.isTrigger = true;
        //    damageCollider.enabled = false;
        //}

        //public void EnabledDamageCollider()
        //{
        //    damageCollider.enabled = true;
        //}
        

        //public void DisabledDamageCollider()
        //{
        //    damageCollider.enabled = false;
        //}

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (other.tag == "Hittable")
        //    {
        //        PlayerBase playerBase = other.GetComponent<PlayerBase>();

        //        if (playerBase != null)
        //        {
        //            playerBase.TakeDamage(currentDamage);
        //        }
        //    }
        //}
    }
}

