using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] ParticleSystem Conffeti;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Conffeti.Play();
        }
    }
}
