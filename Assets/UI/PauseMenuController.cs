using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    private VisualElement root;
    private Button resumeButton;
    private Button restartButton;
    private Button quitButton;
    private bool isPaused = false;

    void Start()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Find all the buttons
        resumeButton = root.Q<Button>("resume-button");
        restartButton = root.Q<Button>("restart-button");
        quitButton = root.Q<Button>("quit-button");

        // Assign callbacks to buttons
        resumeButton.clicked += ResumeGame;
        restartButton.clicked += RestartGame;
        quitButton.clicked += QuitToMainMenu;

        // Initially hide the pause menu
        root.style.display = DisplayStyle.None;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            root.style.display = DisplayStyle.Flex;
            UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else
        {
            ResumeGame();
        }
    }

    private void ResumeGame()
    {
        root.style.display = DisplayStyle.None;
        Time.timeScale = 1;
        isPaused = false;
        UnityEngine.Cursor.lockState = UnityEngine.CursorLockMode.Locked;
        UnityEngine.Cursor.visible = false;
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1; // Ensure the time scale is reset
        isPaused = false;
    }

    private void QuitToMainMenu()
    {
        Time.timeScale = 1; // Make sure time scale is reset
        SceneManager.LoadScene(0);
    }
}
