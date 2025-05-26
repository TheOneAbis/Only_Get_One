using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerRef : MonoBehaviour
{
    public void LoadScene(string level) 
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(level);
    }
    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
