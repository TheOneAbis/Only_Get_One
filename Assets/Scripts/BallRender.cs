using UnityEngine;

public class BallRender : MonoBehaviour
{
    public float _snapDistance;

    Vector3 snapOffset = Vector3.zero;
    Vector3 spinAxis = Vector3.right;
    float spinVelocity = 90;

    void Update()
    {
        Vector3 targetPosition= transform.position;
        Collider[] colliders  = Physics.OverlapSphere(transform.position, 0.5f + _snapDistance);
        Vector3 mostClosestPoint = Vector3.one;
        foreach(Collider collider in colliders)
        {
            if (collider.gameObject == gameObject) continue;
            Vector3 nextClosestPoint = collider.ClosestPoint(transform.position);
            if (Vector3.Distance(nextClosestPoint, transform.position) < Vector3.Distance(mostClosestPoint, transform.position))
            {
                mostClosestPoint = nextClosestPoint;
            }
        }
        
        Vector3 offset = mostClosestPoint - transform.position;
        float distance = offset.magnitude - 0.5f;
        Vector3 offsetDir = offset.normalized;

        if(distance <= _snapDistance)
        {
 
            snapOffset = Vector3.Lerp(snapOffset, offsetDir * distance, 0.1f);
            //snapOffset = Vector3.ClampMagnitude(snapOffset, distance);
            Vector3 velocity = GetComponent<Rigidbody>().linearVelocity;

            
            spinAxis  = Vector3.Cross(snapOffset, velocity);
            spinVelocity = velocity.magnitude;


        }
        transform.rotation = transform.rotation;
        transform.position = targetPosition+ snapOffset;
    }
}
