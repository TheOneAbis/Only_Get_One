using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerRef : MonoBehaviour
{
    public void LoadScene(string level) 
    {
        SceneManager.LoadScene(level);
    }
    public void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
