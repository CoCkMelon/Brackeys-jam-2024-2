using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject settingsPanel;
    [SerializeField]
    GameObject mainButtons;

    // Start is called before the first frame update
    void Start()
    {
        settingsPanel.SetActive(false);
        mainButtons.SetActive(true);
    }

    public void PlayGame() 
    {
        // Go to Game Scene
    }

    public void OpenSettings() 
    {
        // Make Settings Panel Visible
        mainButtons.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void ExitGame() 
    {
        // Close the game
        Application.Quit();
    }

}
