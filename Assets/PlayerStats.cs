using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;

    public int maxStamina = 100;
    public float currentStamina;

    public int maxBreath = 100;
    public float currentBreath;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        currentStamina = maxStamina;

        currentBreath = maxBreath;
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void TakeDamage(float damage)
    {
        if (currentHealth != 0)
        {
            currentHealth -= damage;
        }
    }

    public void LoseStamina(int energy)
    {
        if (currentStamina != 0)
        {
            currentStamina -= energy * Time.deltaTime;
        }
        if(currentStamina < 0)
        {
            currentStamina = 0;
        }
    }

    public void RunOutOfBreath(int air)
    {
        if (currentBreath != 0)
        {
            currentBreath -= air * Time.deltaTime;
        }
        if(currentBreath < 0)
        {
            currentBreath = 0;
        }
    }

    public void RestoreStamina(int energy)
    {
        currentStamina += energy * Time.deltaTime;

        if(currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }

    public void FreshAir(int air)
    {
        currentBreath += air * Time.deltaTime;

        if(currentBreath > maxBreath)
        {
            currentBreath = maxBreath;
        }
    }

    public void RegainHealth(float health)
    {
        currentHealth += health;

        if(currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
