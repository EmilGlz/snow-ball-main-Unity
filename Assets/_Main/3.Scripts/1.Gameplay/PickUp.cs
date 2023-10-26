using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] ParticleSystem PickUpParticle;
    [SerializeField] GameObject Object;
    AudioSource PickUpSound;

    private void Start()
    {
        PickUpSound = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(Object);
            PickUpParticle.Play();
            PickUpSound.Play();
        }
        
    }

}
