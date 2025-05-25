using UnityEngine;

public class BallRender : MonoBehaviour
{
    [SerializeField]
    GameObject _target;


    // Update is called once per frame
    void Update()
    {
        transform.position = _target.transform.position;
    }
}
