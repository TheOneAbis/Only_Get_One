using UnityEngine;

public class Collectable : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        Data.Points++;

        GetComponent<AudioSource>().Play();

        GetComponent<SphereCollider>().enabled = false;
        transform.GetChild(0).gameObject.SetActive(false);

        Destroy(gameObject, 1f);
    }
}
