using UnityEngine;

public class PlayHarvesterSOund : MonoBehaviour
{
    [SerializeField] AudioSource HarvesterSound;


    private void OnTriggerEnter(Collider other)
    {
        HarvesterSound.Play();
    }
}
