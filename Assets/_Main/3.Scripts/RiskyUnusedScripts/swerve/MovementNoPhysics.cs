using UnityEngine;

public class MovementNoPhysics : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Rigidbody rb;

    private Vector3 prevPos = new Vector3();
    private Vector3 currPos = new Vector3();

    private bool isGrounded;

    public float speed = 10;
    public float _gravity = 5;


    private float distToGround;
    private Vector3 normal = Vector3.up;


    private void Start()
    {
        distToGround = GetComponent<Collider>().bounds.extents.y - GetComponent<Collider>().bounds.center.y;
    }

    private void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, distToGround + 0.2f))
        {
            normal = hit.normal;
        }
    }

    void FixedUpdate()
    {
        GroundCheck();

        if (!isGrounded)
        {
            rb.AddForce(Vector3.down * _gravity);

        }

        if (isGrounded)
        {
            rb.velocity = Vector3.ProjectOnPlane(rb.velocity, normal);
            //rb.AddForce(0,  -15, speed);

            foreach (Touch t in Input.touches)
            {
                if (t.phase == TouchPhase.Began)
                {
                    prevPos = t.position;
                }
                if (t.phase == TouchPhase.Moved)
                {
                    currPos = t.position;
                    float deltaX = currPos.x - prevPos.x;
                    bool swipedSideways = Mathf.Abs(deltaX) > 2.5f;
                    if (swipedSideways && player.position.x < 2 && player.position.x > -2)
                    {
                        if (deltaX < 0)
                        {
                            player.transform.Translate(-0.2f, 0, 0);
                            //rb.AddForce(-19, 0, 0);
                        }
                        else if (deltaX > 0)
                        {
                            player.transform.Translate(0.2f, 0, 0);
                            //rb.AddForce(19, 0, 0);
                        }
                    }
                    prevPos = currPos;
                }
            }
        }

        if (player.position.x >= 2)
        {
            player.transform.Translate(-0.1f, 0, 0);
        }
        else if (player.position.x <= -2)
        {
            player.transform.Translate(0.1f, 0, 0);
        }
    }

    public void Freeze()
    {
        rb.constraints = RigidbodyConstraints.FreezePosition;
    }


    void GroundCheck()
    {
        RaycastHit hit;
        float distance = 1f;
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

    public Vector3 Align(Vector3 vector, Vector3 normal)
    {
        //to rotate a movement vector by a surface normal
        Vector3 tangent = Vector3.Cross(normal, vector);
        Vector3 newVector = -Vector3.Cross(normal, tangent);
        vector = newVector.normalized * vector.magnitude;
        return vector;
    }

}