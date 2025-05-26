using System.Collections;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    float maxHeight = 10;
    float speed = 4;
    Vector3 startPos;
    float rotateSpeed = 90;
    public float timeOffset = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startPos = transform.position;
        fliping = true;
        Invoke("StartFlip", 1f);
        if(timeOffset>= maxHeight)
        {
            transform.rotation = transform.rotation * Quaternion.AngleAxis(180, Vector3.right);
        }
    }

    public void StartFlip()
    {
        fliping = false;
    }
    // Update is called once per frame
    bool fliping = false;
    void Update()
    {
        float currentPosition = Mathf.PingPong((Time.time+timeOffset) * 1f, maxHeight)*2 ;
        transform.position = startPos + Vector3.up * currentPosition;
        if ((currentPosition >= 19.5f ||currentPosition<=0.5f)&& fliping == false)
        {
            StartCoroutine(Rotate());
            fliping = true;
        }
        
    }
    Quaternion startingRotation;
    IEnumerator Rotate()
    {
        startingRotation = transform.rotation;
        float totalRotation = 0;
        while (totalRotation < 180)
        {
            
            totalRotation += Time.deltaTime * rotateSpeed;
            if (totalRotation > 180) totalRotation = 180;
            transform.rotation = startingRotation * Quaternion.AngleAxis(totalRotation, Vector3.right);
            yield return null;
        }
        fliping = false;
        
    }
}
