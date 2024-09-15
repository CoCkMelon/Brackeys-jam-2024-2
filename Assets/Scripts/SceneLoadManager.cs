using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    // Reference to the player's transform
    public Transform player;

    // Dictionary to hold scene names and their corresponding Z ranges
    private Dictionary<string, Vector2> sceneRanges = new Dictionary<string, Vector2>();

    // HashSet to keep track of currently loaded scenes
    private HashSet<string> loadedScenes = new HashSet<string>();

    // List to keep track of whether each scene is loaded
    private bool[] sceneLoaded = new bool[5];

    void Start()
    {
        // Initialize the scene ranges
        // For example, Scene1 is active when player's Z is between 0 and 100
        sceneRanges.Add("LEVEL 1", new Vector2(0f, 2000f));
        sceneRanges.Add("Big island", new Vector2(1000f, 3000f));
        // sceneRanges.Add("Another island", new Vector2(100f, 200f));
        sceneRanges.Add("Rocky island", new Vector2(4000f, 6000f));
        sceneRanges.Add("LAST LEVEL", new Vector2(8000f, 16000f));

        // Initialize the sceneLoaded array to false
        for (int i = 0; i < sceneLoaded.Length; i++)
        {
            sceneLoaded[i] = false;
        }
    }

    void Update()
    {
        float playerZ = player.position.z;

        for (int i = 0; i < sceneRanges.Count; i++)
                {
            string sceneName = GetSceneName(i);
            float zMin = sceneRanges[sceneName].x;
            float zMax = sceneRanges[sceneName].y;

            bool isInRange = playerZ >= zMin && playerZ < zMax;

            if (isInRange && !loadedScenes.Contains(sceneName) && !sceneLoaded[i])
            {
                // Begin loading the scene asynchronously
                StartCoroutine(LoadSceneAsync(sceneName));
                sceneLoaded[i] = true;
            }
            else if (!isInRange && loadedScenes.Contains(sceneName) && sceneLoaded[i])
            {
                // Begin unloading the scene asynchronously
                StartCoroutine(UnloadSceneAsync(sceneName));
                sceneLoaded[i] = false;
            }
        }
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // Start loading the scene additively
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Add the scene to the loaded scenes set
        loadedScenes.Add(sceneName);
    }

    IEnumerator UnloadSceneAsync(string sceneName)
    {
        // Start unloading the scene asynchronously
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName(sceneName));

        // Wait until the asynchronous scene fully unloads
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        // Remove the scene from the loaded scenes set
        loadedScenes.Remove(sceneName);
    }

    private string GetSceneName(int index)
    {
        int i = 0;
        foreach (var kvp in sceneRanges)
        {
            if (i == index)
            {
                return kvp.Key;
            }
            i++;
        }
        return "";
    }
}
