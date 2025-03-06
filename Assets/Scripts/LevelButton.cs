using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private string levelName; // Name of the scene to load
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;
    public void LoadLevel()
    {
        if (SceneLoadManager.Instance != null)
        {
            SceneLoadManager.Instance.LoadScene(levelName);
        }
        else
        {
            Debug.LogError("SceneLoadManager is not initialized!");
        }
    }

    public void ExitGame()
    {
        SceneLoadManager.Instance.QuitGame();
    }
}