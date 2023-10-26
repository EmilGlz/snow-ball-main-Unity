using UnityEngine;
public class HumanController : MonoBehaviour
{
    //[SerializeField] GameObject blood;
    //[SerializeField] GameObject player3dObject;
    [SerializeField] float humanSpeed;
    public bool canRun = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("ENDBNLVL")) // if humans reached the end
        {
            canRun = false;
            gameObject.SetActive(false);
        }
    }

    private void FixedUpdate()
    {
        if (canRun)
        {
            transform.position += transform.forward * Time.fixedDeltaTime * humanSpeed;
        }
    }
}
