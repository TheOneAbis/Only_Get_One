using JetBrains.Annotations;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    float xRotation;
    float yRotation;
    [SerializeField]
    GameObject CameraTarget;
    [SerializeField]
    float yPivotOffset;
    [SerializeField]
    public float offsetDistance;
    [SerializeField]
    float maxXRotation = 90;
    [SerializeField]
    float minXRotation = -45;
    public void UpdatePosition(Vector2 input)
    {
        //Convert linear to axis
        xRotation -= input.y;
        yRotation += input.x;
        xRotation = Mathf.Clamp(xRotation, minXRotation, maxXRotation);
    }
    private void Update()
    {
        Vector3 cameraTarget = CameraTarget.transform.position + Vector3.up *yPivotOffset;
        Quaternion direction = Quaternion.Euler(xRotation, yRotation, 0);
        Vector3 offset = direction * Vector3.forward * offsetDistance ;
        if (Physics.Raycast(cameraTarget, offset, out RaycastHit hit, Mathf.Abs(offsetDistance), LayerMask.GetMask("Default")))
        { 
            offset = direction * Vector3.forward * (-hit.distance+0.1f);
            Debug.Log("Cameracollision");
        }
        Camera.main.transform.position = cameraTarget + offset;
        Camera.main.transform.rotation = direction;

    }
}
