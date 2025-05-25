using UnityEngine;

public class BallAudio : MonoBehaviour
{
    AudioSource rollAudio;
    AudioSource bounceAudio;
    Rigidbody rb;
    Vector3 velocity;
    int colliders = 0;

    public AudioClip[] bounceClips;

    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();
        rollAudio = audios[0];
        bounceAudio = audios[1];
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // update rolling volume based on velocity
        velocity = rb.linearVelocity;
        rollAudio.volume = colliders > 0 ? Mathf.Min(velocity.magnitude / 10f, 1.5f) : 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        colliders++;
        
        // When hitting something, play bounce sound at corresponding volume
        bounceAudio.clip = bounceClips[Random.Range(0, bounceClips.Length)];
        bounceAudio.volume = Mathf.Min(Vector3.Dot(velocity / 10f, -collision.contacts[0].normal), 1.5f);
        bounceAudio.pitch = Random.Range(0.85f, 1.15f);
        
        bounceAudio.Play();
    }
    private void OnCollisionExit(Collision collision)
    {
        colliders--;
    }
}
