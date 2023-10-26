using UnityEngine;

public class SwerveSmooth : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Rigidbody rb;

    private Vector3 prevPos = new Vector3();
    private Vector3 currPos = new Vector3();

    [SerializeField, Range(0, 1)] float swerveAmount = 0.1f;

    // Player 
    public float speed = 10;
    public float _gravity = 5;

    // Align Surface 
    public bool grounded;
    private Vector3 posCur;
    private Quaternion rotCur;

    private void Update()
    {
        AlignSurface();
    }

    void FixedUpdate()
    {
        if (grounded)
        {
            rb.velocity = Vector3.forward * speed;

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
                    Debug.Log("deltaX: " + deltaX);
                    bool swipedSideways = Mathf.Abs(deltaX) > 2.5f;
                    if (swipedSideways)
                    {
                        if (deltaX < 0)
                        {
                            player.transform.Translate(-swerveAmount, 0, 0);
                        }
                        else if (deltaX > 0)
                        {
                            player.transform.Translate(swerveAmount, 0, 0);
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


    void AlignSurface()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -transform.up, out hit, 1.5f) == true)
        {
            Debug.DrawLine(transform.position, hit.point, Color.red);

            rotCur = Quaternion.FromToRotation(transform.up, hit.normal) * transform.rotation;
            posCur = new Vector3(transform.position.x, hit.point.y + 0.05f, transform.position.z);

            grounded = true;
        }
        else
        {
            grounded = false;
        }


        if (grounded == true)
        {
            transform.position = Vector3.Lerp(transform.position, posCur, Time.deltaTime * 9);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotCur, Time.deltaTime * 9);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, transform.position - Vector3.up * 1f, Time.deltaTime * 9);

        }
    }
}