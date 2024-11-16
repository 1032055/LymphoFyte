using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    public Slider healthBar;


    private void Start()
    {
        healthBar = GetComponent<Slider>();
    }
    public void SetMaxHealth(float maxHealth)
    {
        healthBar.maxValue = maxHealth;
        healthBar.value = maxHealth;
    }

    public void SetCurrentHealth(float currentHealth)
    {
        healthBar.value = currentHealth;
    }
}
