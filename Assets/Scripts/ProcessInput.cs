using UnityEngine;
using UnityEngine.InputSystem;

public class ProcessInput : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPause(InputValue input)
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
    }
    public void OnLook(InputValue input)
    {
        Vector2 mouseInput = input.Get<Vector2>();
        Debug.Log("Mouse move input: "+ mouseInput);
    }
    public void OnLeftClick(InputValue input)
    {
        string buttonState = input.isPressed ? "Pressed" : "Released";
        Debug.Log("Left Click " + buttonState);
    }
}
