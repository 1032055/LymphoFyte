using EB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EB
{
    public class AnimationRelay : MonoBehaviour
    {
        [Header("Damage Colliders")]
        public Damagecollider rightHandCollider;
        public Damagecollider leftHandCollider;

        public void EnableRightHandCollider()
        {
            if (rightHandCollider != null)
                rightHandCollider.EnableDamageCollider();
        }

        public void DisableRightHandCollider()
        {
            if (rightHandCollider != null)
                rightHandCollider.DisableDamageCollider();
        }

        public void EnableLeftHandCollider()
        {
            if (leftHandCollider != null)
                leftHandCollider.EnableDamageCollider();
        }

        public void DisableLeftHandCollider()
        {
            if (leftHandCollider != null)
                leftHandCollider.DisableDamageCollider();
        }
    }
}

