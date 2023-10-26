using UnityEngine;

public class CombinerStarter : MonoBehaviour
{
    private bool Movement = false;
    [SerializeField] GameObject Harvester;
    void Update()
    {
        if (Movement)
        {
            //Harvester.LeanMoveZ(Harvester.transform.position.z - 8, 15);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Movement = true;
        }
    }
}
