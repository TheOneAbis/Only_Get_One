using UnityEngine;
using UnityEngine.SceneManagement;

public class GameComplete : MonoBehaviour
{
    [SerializeField]
    string _endScene = "End";
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(_endScene);
    }
}
