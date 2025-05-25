using UnityEngine;

public class Launcher : MonoBehaviour
{
    bool _activeOnce = false;
    //The origin is the spawn point of the ball
    ProcessInput _processInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _processInput = FindFirstObjectByType<ProcessInput>();

        
    }
    private void Update()
    {
        float cameraY = Camera.main.transform.rotation.eulerAngles.y;
        transform.rotation = Quaternion.Euler(0, cameraY, 0);
    }

    // Update is called once per frame
    void OnTriggerEnter()
    {
        if (!_activeOnce)
        {
            _processInput.ShotTaken = false;
            _processInput.Ball.isKinematic = true;
            _processInput.Ball.transform.position = transform.position;
            _activeOnce = true;
            Data.Goals.Add(name);
        }
    }
}
