using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool paused = false;
    public GameObject PauseMenuCanvas;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    void Stop()
    {
        if (!paused)
        {
            PauseMenuCanvas.SetActive(true);
            Time.timeScale = 0f;
            paused = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Play()
    {
        if (paused)
        {
            PauseMenuCanvas.SetActive(false);
            Time.timeScale = 1f;
            paused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    public void MainMenuButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
