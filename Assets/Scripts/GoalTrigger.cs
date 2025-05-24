using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    ProcessInput _processInput;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _processInput = FindFirstObjectByType<ProcessInput>();

        
    }

    // Update is called once per frame
    void OnTriggerEnter()
    {
        _processInput.ShotTaken = false;
    }
}
