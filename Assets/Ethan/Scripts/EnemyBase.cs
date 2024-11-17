using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 100f;
    public float currentHealth;
    public float speed = 10f;
    public float Damage = 10f;

    public HealthBar healthBar;

    Animator animationHandler;


    private void Awake()
    {
        animationHandler = GetComponentInChildren<Animator>();
    }

    public void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }


    public void TakeDamage(float damage)
    {
        currentHealth = currentHealth - damage;

        healthBar.SetCurrentHealth(currentHealth);

        animationHandler.Play("GetHit");

        if (currentHealth <= 0)
        {
            currentHealth = 0;
            animationHandler.Play("KnockDown");
        }
    }
}
