using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Tilemaps;

public class SceneLoadManager : MonoBehaviour
{
    public static SceneLoadManager Instance { get; private set; }

    [SerializeField] private GameObject loadingScreenUI; // Assign via Inspector
    [SerializeField] private UnityEngine.UI.Slider progressBar; // Optional for progress bar
    [SerializeField] private float forcedLoadingTime = 1f;
    [SerializeField] private AudioSource backgroundAudioSource;
    private bool _backgroundMusicFadingIn = false;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            // Destroy duplicate instance if one already exists
            Destroy(gameObject);
            Debug.Log("Destroying duplicate instance of LoadingScreenManager.");
        }
        else
        {
            // Set this as the singleton instance and prevent it from being destroyed
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("LoadingScreenManager is not destroyed and persists between scenes.");
        }
        backgroundAudioSource.loop = true;
        backgroundAudioSource.Play();
        loadingScreenUI.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        if (sceneName == "LevelSelectScene" || sceneName == "MainMenuScene")
        {
            StartCoroutine(MusicFadeIn());
        }
        else
        {
            StartCoroutine(MusicFadeOut());
        }
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Show loading screen
        loadingScreenUI.SetActive(true);

        // Load scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        
        // Update progress bar if applicable
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f); // Normalize progress to 0-1
            if (progressBar != null)
                progressBar.value = progress;

            // Allow activation when load is nearly done
            if (operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
        
        yield return new WaitForSeconds(forcedLoadingTime);
        // Hide loading screen
        loadingScreenUI.SetActive(false);
    }
    public void QuitGame()
    {
        loadingScreenUI.SetActive(true);
        Application.Quit();
    }

    private IEnumerator MusicFadeIn()
    {
        if (backgroundAudioSource.gameObject.activeInHierarchy) yield break;
        _backgroundMusicFadingIn = true;
        backgroundAudioSource.gameObject.SetActive(true);
        backgroundAudioSource.Play();
        while (backgroundAudioSource.volume < 0.75f)
        {
            if (_backgroundMusicFadingIn == false) break;
            backgroundAudioSource.volume += 1f * Time.deltaTime;
            yield return null;
        }
        backgroundAudioSource.volume = 0.75f;
    }
    private IEnumerator MusicFadeOut()
    {
        _backgroundMusicFadingIn = false;
        while (backgroundAudioSource.volume > 0f)
        {
            if (_backgroundMusicFadingIn == true) break;
            backgroundAudioSource.volume -= 1f * Time.deltaTime;
            yield return null;
        }
        backgroundAudioSource.volume = 0f;
        backgroundAudioSource.gameObject.SetActive(false);
    }
}