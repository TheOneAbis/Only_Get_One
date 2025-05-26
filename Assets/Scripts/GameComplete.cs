using UnityEngine;
using UnityEngine.SceneManagement;

public class GameComplete : MonoBehaviour
{
    [SerializeField]
    string _endScene = "End";
    private void OnTriggerEnter(Collider other)
    {
        Invoke("LoadScore", 3f);
    }
    private void LoadScore()
    {
        SceneManager.LoadScene(_endScene);
    }
}
