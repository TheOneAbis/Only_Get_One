using UnityEngine;

public class RippleDecal : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject ball;
    void Start()
    {
        transform.position = ball.transform.position - new Vector3(0, 0.9f, 0);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ball.transform.position - new Vector3(0, 0.9f, 0);
    }
}
