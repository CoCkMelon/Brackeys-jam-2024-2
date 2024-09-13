using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStats : MonoBehaviour
{
    public int maxHealth = 100;
    public float currentHealth;

    public int maxStamina = 100;
    public float currentStamina;

    public int maxBreath = 100;
    public float currentBreath;

    public string additionalSceneToLoad = "Rocky island";  // The name of the scene to be loaded asynchronously
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;

        currentStamina = maxStamina;

        currentBreath = maxBreath;
    }


    public void TakeDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
        }
        if (currentHealth <= 0){
            RestartLevelAndLoadAdditionalScene();
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


    void RestartLevelAndLoadAdditionalScene()
    {
        // Get the active scene (current level)
        Scene currentScene = SceneManager.GetActiveScene();

        // Restart the current scene
        SceneManager.LoadScene(currentScene.name);

        // Start async loading of the additional scene
        StartCoroutine(LoadAdditionalSceneAsync());
    }

    System.Collections.IEnumerator LoadAdditionalSceneAsync()
    {
        // Load the additional scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(additionalSceneToLoad, LoadSceneMode.Additive);

        // Wait until the additional scene is fully loaded
        while (!asyncLoad.isDone)
        {
            yield return null;  // Wait for the next frame
        }

        Debug.Log("Additional scene loaded successfully.");
    }
}
