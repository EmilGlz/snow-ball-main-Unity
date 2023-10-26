using UnityEngine;

public class CombinerController : MonoBehaviour
{
    [SerializeField] Transform snowballMesh;
    [SerializeField] Rigidbody rb;
    [SerializeField] float combinerSpeed;

    public int snowBallLevelToGameOver;

    private bool isGrounded;


    private void Start()
    {
        // StartMoving();
    }

    private void Update()
    {
        GroundCheck();

        if (isGrounded)
        {
            rb.velocity = transform.forward * combinerSpeed;
        }
        else
        {
            rb.AddForce(Vector3.down * 5);
        }
    }

    public void StartMoving()
    {
        //Debug.Log("Started moving: " + gameObject.name );
        rb.velocity = transform.forward * combinerSpeed;
    }

    public void StopMoving()
    {
        rb.velocity = Vector3.zero;
    }

    void GroundCheck()
    {
        RaycastHit hit;
        float distance = 10f;
        Vector3 dir = new Vector3(0, -1);

        if (Physics.Raycast(transform.position, dir, out hit, distance))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
