using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

public class ProcessInput : MonoBehaviour
{
    [SerializeField] GameObject _pauseMenu;
    [SerializeField] GameObject _failPrompt;
    [SerializeField] CameraController _cameraController;
    public Rigidbody Ball;
    public DecalProjector Ripple;

    bool charge;
    bool chargeup;
    float chargeMult = 0.5f;
    float minCharge = 0.1f;
    float chargeTime;

    [SerializeField] float launchForce;
    float launchTime;

    [SerializeField] float _minVelocity = 0.1f;
    [HideInInspector] public bool ShotTaken = false;
    [SerializeField] GameObject _forceArrow;
 
    [SerializeField] float arrowMaxScaleMult = 2f;
    float arrowMinScale;

    public static UnityEvent<float> onChargeBegin = new();
    public static UnityEvent<float> onChargeUpdated = new();
    public static UnityEvent onChargeCancelled = new();
    public static UnityEvent onChargeRelease = new();

    public float startingFOV = 60;
    public float endFOV= 50;
    public float speedFOV = 90;
    float fovVel;

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
        arrowMinScale = _forceArrow.transform.localScale.z;
    }
    public void OnPause(InputValue input)
    {
        _pauseMenu.SetActive(!_pauseMenu.activeSelf);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = _pauseMenu.activeSelf;
        Time.timeScale = _pauseMenu.activeSelf ? 0 : 1;
    }
    public void OnRestart(InputValue input)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnLook(InputValue input)
    {
        if (_pauseMenu.activeSelf) return;

        Vector2 mouseInput = input.Get<Vector2>() / 4f;
        _cameraController.UpdatePosition(mouseInput);
    }

    public void OnLeftClick(InputValue input)
    {
        if (_pauseMenu.activeSelf) return;

        if (ShotTaken)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            return;
        }

        if (!charge && input.isPressed)
            onChargeBegin?.Invoke(chargeMult);

        charge = input.isPressed;
        //Debug.Log("Left Click " + charge);
    }

    public void OnRightClick(InputValue input)
    {
        if (_pauseMenu.activeSelf) return;

        if (charge && input.isPressed)
        {
            charge = false;
            onChargeCancelled?.Invoke();
            chargeTime = 0;
        }
    }
   
    private void Update()
    {
        float targetFov = 60;
        if (ShotTaken == false)
        {
            _forceArrow.SetActive(charge);

            Vector3 dir = (Camera.main.transform.position + Camera.main.transform.forward *
                (Mathf.Abs(_cameraController.GetComponent<CameraController>().offsetDistance) * 2.25f) - Ball.transform.position).normalized;
            
            if (charge)
            {
                _forceArrow.transform.rotation = Quaternion.LookRotation(dir, Vector3.up);

                Vector3 newScale = _forceArrow.transform.localScale;
                newScale.z = Mathf.Lerp(arrowMinScale, arrowMinScale * arrowMaxScaleMult, chargeTime);
                _forceArrow.transform.localScale = newScale;
                Debug.Log(newScale);

                targetFov = Mathf.Lerp(startingFOV, endFOV, chargeTime);

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
                onChargeUpdated?.Invoke(chargeTime);
            }
            else
            {
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
        }
        else
        {
            float speed = Ball.linearVelocity.magnitude;
            float maxSpeed = 20;
            
            targetFov = Mathf.Lerp(startingFOV, speedFOV, speed / maxSpeed);
            if (Ball.linearVelocity.magnitude <= _minVelocity && Time.time - launchTime > 3.0f)
                _failPrompt.SetActive(true);

            Ripple.gameObject.SetActive(Ball.GetComponent<Rigidbody>().linearVelocity.sqrMagnitude <= 0.1);
        }

        Camera.main.fieldOfView = Mathf.SmoothDamp(Camera.main.fieldOfView, targetFov, ref fovVel, 0.2f);
    }
}
