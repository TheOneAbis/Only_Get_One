using UnityEngine;

public class BallAudio : MonoBehaviour
{
    AudioSource rollAudio;
    AudioSource bounceAudio;
    AudioSource chargeAudio;
    Rigidbody rb;

    Vector3 velocity;
    int colliders = 0;

    public AudioClip[] bounceClips;
    public AudioClip puntClip, chargeClip;

    void Start()
    {
        AudioSource[] audios = GetComponents<AudioSource>();
        rollAudio = audios[0];
        bounceAudio = audios[1];
        chargeAudio = audios[2];
        rb = GetComponent<Rigidbody>();

        ProcessInput.onChargeBegin.AddListener((chargeMultiplier) =>
            {
                chargeAudio.clip = chargeClip;
                chargeAudio.volume = 0;
                chargeAudio.pitch = 1;
                chargeAudio.loop = true;
                chargeAudio.Play();
            });
        ProcessInput.onChargeUpdated.AddListener((chargeTime) =>
        {
            chargeAudio.volume = Mathf.Lerp(0f, 0.5f, chargeTime);
            chargeAudio.pitch = Mathf.Lerp(1f, 1.5f, chargeTime);
        });
        ProcessInput.onChargeRelease.AddListener(() =>
            {
                chargeAudio.clip = puntClip;
                chargeAudio.volume = 1;
                chargeAudio.pitch = 1;
                chargeAudio.loop = false;
                chargeAudio.Play();
            });
        ProcessInput.onChargeCancelled.AddListener(() => chargeAudio.Stop());
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
