using Unity.VisualScripting;
using UnityEngine;

public class BallAudio : MonoBehaviour
{
    AudioSource rollAudio;
    AudioSource bounceAudio;
    Rigidbody rb;

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
        // upodate rolling volume based on velocity
        rollAudio.volume = Mathf.Min(rb.linearVelocity.magnitude / 10f, 1.5f);
        Debug.Log(rollAudio.volume);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // When hitting something, play bounce sound at corresponding volume
        bounceAudio.clip = bounceClips[Random.Range(0, bounceClips.Length)];
        bounceAudio.volume = Mathf.Min(rb.linearVelocity.magnitude / 10f, 1.5f);
        bounceAudio.Play();
    }
}
