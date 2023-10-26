using UnityEngine;

public class MovingSphereEL : MonoBehaviour
{

    [SerializeField, Range(0f, 100f)]
    public float maxSpeed = 10f;

    [SerializeField, Range(0f, 100f)]
    float maxAcceleration = 10f, maxAirAcceleration = 1f;

    [SerializeField, Range(0, 90)]
    float maxGroundAngle = 25f;

    [SerializeField, Range(0f, 100f)]
    float maxSnapSpeed = 100f;

    [SerializeField, Min(0f)]
    float probeDistance = 1f;

    [SerializeField]
    LayerMask probeMask = -1;

    Rigidbody body;

    public Vector3 velocity, desiredVelocity;

    Vector3 contactNormal;

    int groundContactCount;

    bool OnGround => groundContactCount > 0;

    float minGroundDotProduct;

    int stepsSinceLastGrounded;


    bool nitroOn = false;
    SwerveMovementEL swerveMovement;

    // [SerializeField] SwerveMovement swerveMovement;

    [SerializeField] SwerveInputSystemEL _swerveInputSystem;

    [SerializeField] private float swerveSpeed = 0.5f;
    [SerializeField] private float maxSwerveAmount = 1f;

    //private float swerveAmount;

    // --------------
    [SerializeField] Transform player;
    [SerializeField] Transform playerMesh;
    [SerializeField] Rigidbody rb;

    private Vector3 prevPos = new Vector3();
    private Vector3 currPos = new Vector3();

    [SerializeField, Range(0, 100)] float swerveAmount = 0.1f;
    [SerializeField] float rotSpeed;
    // Player 
    public float speed = 10;
    public float _gravity = 5;

    // Align Surface 
    public bool grounded;

    [SerializeField] float velocityX;
    [SerializeField] float deltaX;
    // --------------




    void OnValidate()
    {
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    void Awake()
    {
        swerveMovement = GetComponent<SwerveMovementEL>();
        body = GetComponent<Rigidbody>();
        OnValidate();
    }

    private void Start()
    {
        body.velocity = new Vector3(0,1,0);
    }


    void FixedUpdate()
    {


        Vector2 playerInput;
        playerInput.y = 1;
        playerInput.x = 0;

        //playerInput.x = swerveAmount;
        //playerInput.x= 0;

        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                prevPos = t.position;
            }
            else if (t.phase == TouchPhase.Moved)
            {
                currPos = t.position;
                deltaX = currPos.x - prevPos.x;
                bool swipedSideways = Mathf.Abs(deltaX) > 5.5f;
                if (swipedSideways)
                {
                    playerInput.x = deltaX * Time.deltaTime * speed;

                    //if (deltaX < 0)
                    //{

                    //    //playerInput.x = swerveAmount * -1f;
                    //    //rb.velocity = Vector3.right * swerveAmount * -1f;
                    //}
                    //else if (deltaX > 0)
                    //{
                    //    playerInput.x = deltaX * Time.deltaTime * speed;
                    //    //playerInput.x = swerveAmount;
                    //    //rb.velocity = Vector3.right * swerveAmount;
                    //}
                }
                prevPos = currPos;
            }

            else if (t.phase == TouchPhase.Ended)
            {
                playerInput.x = 0;
                deltaX = 0;
            }

        }

        desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        UpdateState();
        AdjustVelocity();


        //body.velocity = velocity;
        body.velocity = new Vector3(velocity.x, velocity.y, velocity.z);
        ClearState();
    }

    void ClearState()
    {
        groundContactCount = 0;
        contactNormal = Vector3.zero;
    }

    void UpdateState()
    {
        stepsSinceLastGrounded += 1;
        velocity = body.velocity;
        if (OnGround || SnapToGround())
        {
            stepsSinceLastGrounded = 0;
            if (groundContactCount > 1)
            {
                contactNormal.Normalize();
            }
        }
        else
        {
            contactNormal = Vector3.up;
        }
    }

    bool SnapToGround()
    {
        if (stepsSinceLastGrounded > 1)
        {
            return false;
        }
        float speed = velocity.magnitude;
        if (speed > maxSnapSpeed)
        {
            return false;
        }
        if (!Physics.Raycast(
            body.position, Vector3.down, out RaycastHit hit,
            probeDistance, probeMask
        ))
        {
            return false;
        }
        if (hit.normal.y < minGroundDotProduct)
        {
            return false;
        }

        groundContactCount = 1;
        contactNormal = hit.normal;
        float dot = Vector3.Dot(velocity, hit.normal);
        if (dot > 0f)
        {
            velocity = (velocity - hit.normal * dot).normalized * speed;
        }
        return true;
    }

    void AdjustVelocity()
    {
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        float currentX = Vector3.Dot(velocity, xAxis);
        float currentZ = Vector3.Dot(velocity, zAxis);

        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        float maxSpeedChange = acceleration * Time.deltaTime;

        float newX =
            Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        float newZ =
            Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    void EvaluateCollision(Collision collision)
    {
        float minDot = minGroundDotProduct;
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            if (normal.y >= minDot)
            {
                groundContactCount += 1;
                contactNormal += normal;
            }
        }
    }

    Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - contactNormal * Vector3.Dot(vector, contactNormal);
    }
}
