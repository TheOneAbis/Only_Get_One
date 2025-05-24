using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProcessInput : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private Rigidbody _ball;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPause(InputValue input)
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
    }
    public void OnLook(InputValue input)
    {
        Vector2 mouseInput = input.Get<Vector2>();
        Debug.Log("Mouse move input: "+ mouseInput);
        _cameraController.UpdatePosition(mouseInput);
    }

    bool charge;
    public void OnLeftClick(InputValue input)
    {
        charge = input.isPressed;
        string buttonState = charge ? "Pressed" : "Released";
        Debug.Log("Left Click " + buttonState);
        charge = input.isPressed;
    }

    float chargeMult = 0.5f;
    float minCharge = 0.1f;
    float chargeTime;
    [SerializeField]
    float launchForce;
    private void Update()
    {
        if (charge)
        {
            chargeTime += Time.deltaTime * chargeMult;
            chargeTime = Mathf.Min(chargeTime, 1);
        }
        else
        {
            if (chargeTime > minCharge)
            {
                _ball.AddForce(Camera.main.transform.forward * chargeTime*launchForce,ForceMode.Impulse);
            }
            chargeTime = 0;
        }
    }
}
