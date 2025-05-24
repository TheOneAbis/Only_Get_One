using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
public class ProcessInput : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _failMenu;
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    private Rigidbody _ball;



    bool charge;
    float chargeMult = 0.5f;
    float minCharge = 0.1f;
    float chargeTime;

    [SerializeField]
    float launchForce;

    [SerializeField]
    float _minVelocity = 0.1f;
    bool _shotTaken = false;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void OnPause(InputValue input)
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
    }
    public void OnRestart(InputValue input)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnLook(InputValue input)
    {
        Vector2 mouseInput = input.Get<Vector2>();
        Debug.Log("Mouse move input: "+ mouseInput);
        _cameraController.UpdatePosition(mouseInput);
    }

    public void OnLeftClick(InputValue input)
    {
        charge = input.isPressed;
        string buttonState = charge ? "Pressed" : "Released";
        Debug.Log("Left Click " + buttonState);
        charge = input.isPressed;
        if (_shotTaken)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


    private void Update()
    {
        if (_shotTaken == false)
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
                    _ball.AddForce(Camera.main.transform.forward * chargeTime * launchForce, ForceMode.Impulse);
                    _shotTaken = true;
                }
                chargeTime = 0;
            }
        }
        else
        {
            if (_ball.linearVelocity.magnitude <= _minVelocity)
            {
                
            }
        }


    }
}
