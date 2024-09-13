using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{

    [SerializeField]
    GameObject settingsPanel;
    [SerializeField]
    GameObject mainButtons;


    public void BackButton() 
    {
        mainButtons.SetActive(true);
        settingsPanel.SetActive(false);
    }
}
