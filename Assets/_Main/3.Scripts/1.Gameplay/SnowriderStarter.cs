using UnityEngine;

public class SnowriderStarter : MonoBehaviour
{

    [SerializeField] GameObject Snowrider;
    [SerializeField] Animator SnowriderAnim1;
    public bool isOnRightSide;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SnowriderAnim1.Play("SnowR1");
        }
    }
}
