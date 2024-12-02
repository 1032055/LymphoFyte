using EB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBase : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float speed = 10f;
    public float Damage = 10f;

    public HealthBar healthBar;

    Animator animationHandler;
    FightingCombo fightingCombo;
    PlayerLocomotion playerLocomotion;
    Collider playerCollider;
    SFXHandler sfxHandler;

    public bool isStunned;

    private void Awake()
    {
        animationHandler = GetComponentInChildren<Animator>();
        fightingCombo = GetComponentInChildren<FightingCombo>();
        playerLocomotion = GetComponent<PlayerLocomotion>();
        playerCollider = GetComponent<Collider>();
    }

    public void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (isStunned)
        {
            return;
        }
    }

    public void TakeDamage(float damage)
    {
        currentHealth = currentHealth - damage;

        healthBar.SetCurrentHealth(currentHealth);

        animationHandler.Play("GetHit");

        sfxHandler.audioSource.clip = sfxHandler.ough;
        sfxHandler.audioSource.Play();

        if(currentHealth <= 0)
        {
            currentHealth = 0;
            animationHandler.Play("KnockDown");
            playerCollider.enabled = false;

            sfxHandler.audioSource.clip = sfxHandler.DeathSound;
            sfxHandler.audioSource.Play();

        }
        else
        {
            StartCoroutine(Stun(20f));
        }


    }

    private IEnumerator Stun(float duration)
    {
        isStunned = true;

        if (fightingCombo != null)
        {
            fightingCombo.isStunned = true;
        }

        if (playerLocomotion != null)
        {
            playerLocomotion.isStunned = true;
        }

        Debug.Log("Player movement disabled");

        yield return new WaitForSeconds(0.5f);

        Debug.Log("Player movement is enabled");

        if (fightingCombo != null)
        {
            fightingCombo.isStunned = false;
        }

        if (playerLocomotion != null)
        {
            playerLocomotion.isStunned = false;
        }

        isStunned = false;
    }

}
