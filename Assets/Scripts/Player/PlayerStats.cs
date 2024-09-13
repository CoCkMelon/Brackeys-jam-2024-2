using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;

    public int maxStamina = 100;
    public float currentStamina;

    public int maxBreath = 100;
    public float currentBreath;

    public Image healthImage;  // Reference to the health image
    public Image staminaImage; // Reference to the stamina image
    public Image breathImage;  // Reference to the breath image

    public string additionalSceneToLoad = "Rocky island";  // The name of the scene to be loaded asynchronously

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        currentBreath = maxBreath;

        // Initialize the images' transparency
        UpdateHealthTransparency();
        UpdateStaminaTransparency();
        UpdateBreathTransparency();
    }

    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            UpdateHealthTransparency();
        }
        if (currentHealth <= 0)
        {
            RestartLevelAndLoadAdditionalScene();
        }
    }

    public void LoseStamina(int energy)
    {
        if (currentStamina != 0)
        {
            currentStamina -= energy * Time.deltaTime;
            UpdateStaminaTransparency();
        }
        if (currentStamina < 0)
        {
            currentStamina = 0;
            UpdateStaminaTransparency();
        }
    }

    public void RunOutOfBreath(int air)
    {
        if (currentBreath != 0)
        {
            currentBreath -= air * Time.deltaTime;
            UpdateBreathTransparency();
        }
        if (currentBreath < 0)
        {
            currentBreath = 0;
            UpdateBreathTransparency();
        }
    }

    public void RestoreStamina(int energy)
    {
        currentStamina += energy * Time.deltaTime;
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
        UpdateStaminaTransparency();
    }

    public void FreshAir(int air)
    {
        currentBreath += air * Time.deltaTime;
        if (currentBreath > maxBreath)
        {
            currentBreath = maxBreath;
        }
        UpdateBreathTransparency();
    }

    public void RegainHealth(float health)
    {
        currentHealth += health;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UpdateHealthTransparency();
    }

    void UpdateHealthTransparency()
    {
        float alpha = Mathf.Lerp(0.5f, 0.0f, currentHealth / maxHealth);
        Color color = healthImage.color;
        color.a = alpha;
        healthImage.color = color;
    }

    void UpdateStaminaTransparency()
    {
        float alpha = Mathf.Lerp(0.5f, 0.0f, currentStamina / maxStamina);
        Color color = staminaImage.color;
        color.a = alpha;
        staminaImage.color = color;
    }

    void UpdateBreathTransparency()
    {
        float alpha = Mathf.Lerp(0.8f, 0.0f, currentBreath / maxBreath);
        Color color = breathImage.color;
        color.a = alpha;
        breathImage.color = color;
    }

    void RestartLevelAndLoadAdditionalScene()
    {
        // Get the active scene (current level)
        Scene currentScene = SceneManager.GetActiveScene();

        // Restart the current scene
        SceneManager.LoadScene(currentScene.name);
    }
}
