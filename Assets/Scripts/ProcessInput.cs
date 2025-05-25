using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.Windows;

public class ProcessInput : MonoBehaviour
{
    [SerializeField]
    private GameObject _pauseMenu;
    [SerializeField]
    private GameObject _failMenu;
    [SerializeField]
    private CameraController _cameraController;
    [SerializeField]
    public Rigidbody Ball;
    public DecalProjector Ripple;

    bool charge;
    bool chargeup;
    float chargeMult = 0.5f;
    float minCharge = 0.1f;
    float chargeTime;

    [SerializeField]
    float launchForce;
    float launchTime;

    [SerializeField]
    float _minVelocity = 0.1f;
    public bool ShotTaken = false;
    [SerializeField]
    private GameObject _forceArrow;
    [SerializeField]
    private Vector3 _arrowMaxScale = new Vector3(1,1,2);

    public static UnityEvent<float> onChargeBegin = new();
    public static UnityEvent<float> onChargeUpdated = new();
    public static UnityEvent onChargeCancelled = new();
    public static UnityEvent onChargeRelease = new();

    public float startingFOV = 60;
    public float endFOV= 50;
    public float speedFOV = 90;
    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        Ripple.transform.position = Ball.transform.position - new Vector3(0, 0.9f, 0);
        Ripple.gameObject.SetActive(false);
    }
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
        Vector2 mouseInput = input.Get<Vector2>() / 4f;
        //Debug.Log("Mouse move input: "+ mouseInput);
        _cameraController.UpdatePosition(mouseInput);
    }

    public void OnLeftClick(InputValue input)
    {
        if (!charge && input.isPressed)
        {
            onChargeBegin?.Invoke(chargeMult);
        }

        charge = input.isPressed;
        string buttonState = charge ? "Pressed" : "Released";
        Debug.Log("Left Click " + buttonState);
       
        charge = input.isPressed;
        if (ShotTaken)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void OnRightClick(InputValue input)
    {
        if (charge && input.isPressed)
        {
            charge = false;
            onChargeCancelled?.Invoke();
            chargeTime = 0;
        }
    }
    float fovVel;
    private void Update()
    {
        float targetFov = 60;
        Ripple.transform.position = Ball.transform.position - new Vector3(0, 0.9f, 0);
        if (ShotTaken == false)
        {
            _failMenu.SetActive(false);

            Vector3 dir = (Camera.main.transform.position + Camera.main.transform.forward *
            (Mathf.Abs(_cameraController.GetComponent<CameraController>().offsetDistance) * 2.25f) - Ball.transform.position).normalized;

            if (charge)
            {
                _forceArrow.SetActive(true);
                _forceArrow.transform.position = Ball.transform.position+ Vector3.up*0.5f;
                _forceArrow.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);
                _forceArrow.transform.localScale = Vector3.Lerp(Vector3.one,_arrowMaxScale,chargeTime);
                targetFov= Mathf.Lerp(startingFOV, endFOV, chargeTime);
                chargeTime += Time.deltaTime * chargeMult * (chargeup ? 1f : -1f);
                if (chargeTime > 1)
                {
                    chargeTime = 1;
                    chargeup = false;
                }
                if(chargeTime < 0)
                {
                    chargeTime = 0;
                    chargeup = true;
                }
                //chargeTime = Mathf.Min(chargeTime, 1);
                onChargeUpdated?.Invoke(chargeTime);
            }
            else
            {
                _forceArrow.SetActive(false);
                if (chargeTime > minCharge)
                {
                    launchTime = Time.time;
                    onChargeRelease?.Invoke();

                    Ball.isKinematic = false;
                    Ball.AddForce(dir * chargeTime * launchForce, ForceMode.Impulse);
                    ShotTaken = true;
                }
                else onChargeCancelled?.Invoke();

                chargeTime = 0;
                chargeup = true;
            }
            Ripple.gameObject.SetActive(false);
        }
        else
        {
            float speed = Ball.linearVelocity.magnitude;
            float maxSpeed = 20;
            
            targetFov = Mathf.Lerp(startingFOV, speedFOV, speed / maxSpeed);
            if (Ball.linearVelocity.magnitude <= _minVelocity&& Time.time - launchTime>3.0f)
            {
                _failMenu.SetActive(true);
            }
            if (Ball.GetComponent<Rigidbody>().linearVelocity.sqrMagnitude > 0.1)
            {
                Ripple.gameObject.SetActive(false);
            }
            else
            {
                Ripple.gameObject.SetActive(true);
            }
        }

        Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, targetFov, ref fovVel, 0.2f);
    }
}
