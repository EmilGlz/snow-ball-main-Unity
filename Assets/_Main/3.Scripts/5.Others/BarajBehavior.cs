using UnityEngine;
using EZCameraShake;

public class BarajBehavior : MonoBehaviour
{
    [SerializeField] [Range(0, 10)] int levelToBeCracked;
    [SerializeField] ParticleSystem barajParticle;
    [SerializeField] GameObject treesObjects;
    [SerializeField] AudioSource DestroyBarageSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 3)
        {
            Grow grow = other.GetComponent<Grow>();
            //Debug.Log("" + other.collider.name);
            if (grow.growLevel >= levelToBeCracked)
            {
                treesObjects.SetActive(false);
                barajParticle.Play();
                CameraShaker.Instance.ShakeOnce(2f, 2f, .1f, 1f);
                //DestroyBarageSound.Play();
            }
            else
            {
                Debug.Log("Died, collided with tree");
                grow.DIE();
            }
        }
    }
}
