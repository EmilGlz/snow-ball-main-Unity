using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwerveController : MonoBehaviour
{
    [SerializeField] Transform humanCorpsParentTransform;
    [SerializeField] Transform playerMeshTransform;
    [SerializeField] Grow grow;
    [SerializeField] AnimationCurve ascendDescendJumpCurve;
    [SerializeField] AnimationCurve tramplinJumpCurve;
    [SerializeField] AnimationCurve afterRailCurve;
    [SerializeField] Transform treeSpritesParent;
    private Vector2 lastTouchPos;
    private Vector2 currentTouchPos;
    public Vector3 playerPosBeforeJump;
    [Space(10)]
    [SerializeField] float roadWidth;
    [Range(0f, 2f)]
    [SerializeField] float sideMoveSensitivity;
    [SerializeField] float horizontalrotationSpeed;
    [SerializeField] float forwardRotationSpeed;
    [SerializeField] float horizontalRotationSpeed;
    
    [Space(10)]
    public bool canSwerve = true;
    public bool onRail;
    public bool canMoveNormally = true;
    public int movingDirection = 0; // 0 forward, 1  right, 2 backward, 3 left , clockwise
    public int circularMove;
    public float turnRadius;
    public float maxRoadPosRight;
    public float maxRoadPosLeft;
    public float maxRoadPosRightAtStart;
    public float maxRoadPosLeftAtStart;
    public float afterRailSpeed;
    public float circularMovementAngularSpeed = 1f;
    public float circularMovementRadius = 1f;
    public float testTime = 0f;

    private float deltaX;
    [SerializeField] private float sideMove;
    private float screenWidth;
    [HideInInspector]public Transform targetPosToReach;
    [HideInInspector] public Vector3 gameStartPos;
    public float currentAngle;
    MMovement mMovement;
    private Rigidbody rBody;

    void Awake()
    {
        screenWidth = Screen.width;
        rBody = GetComponent<Rigidbody>();
        mMovement = GetComponent<MMovement>();
    }

    private void Start()
    {
        gameStartPos = transform.position;
    }

    private void Update()
    {
        treeSpritesParent.position = new Vector3(transform.position.x, treeSpritesParent.position.y ,transform.position.z);
    }

    private void FixedUpdate()
    {
        if (!canSwerve)
        {
            sideMove = 0;
        }
        else
        {
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);

                if (touch.phase == TouchPhase.Began)
                {
                    lastTouchPos = touch.position;
                }
                // Move the cube if the screen has the finger moving.
                else if (touch.phase == TouchPhase.Moved)
                {
                    currentTouchPos = touch.position;

                    deltaX = lastTouchPos.x - currentTouchPos.x;

                    sideMove = -deltaX / screenWidth * sideMoveSensitivity * roadWidth;

                    lastTouchPos = currentTouchPos;
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Stationary)
                {
                    sideMove = 0;
                }
            }
        }
        if (circularMove == 0)
        {
            if (canMoveNormally)
            {
                if (movingDirection == 0) // forward
                {
                    if (transform.position.x >= maxRoadPosRight)
                    {
                        if (sideMove > 0)
                        {
                            sideMove = 0;
                        }
                        transform.position = new Vector3(maxRoadPosRight, transform.position.y, transform.position.z);
                    }
                    else if (transform.position.x <= maxRoadPosLeft)
                    {
                        if (sideMove < 0)
                        {
                            sideMove = 0;
                        }
                        transform.position = new Vector3(maxRoadPosLeft, transform.position.y, transform.position.z);
                    }
                }
                else if (movingDirection == 1) // right
                {
                    if (transform.position.z <= maxRoadPosRight)
                    {
                        if (sideMove > 0)
                        {
                            sideMove = 0;
                        }
                        transform.position = new Vector3(transform.position.x , transform.position.y, maxRoadPosRight);
                    }
                    else if (transform.position.z >= maxRoadPosLeft)
                    {
                        if (sideMove < 0)
                        {
                            sideMove = 0;
                        }
                        transform.position = new Vector3(transform.position.x, transform.position.y, maxRoadPosLeft);
                    }
                }
                else if (movingDirection == 2) // back
                {
                    if (transform.position.x <= maxRoadPosRight)
                    {
                        if (sideMove > 0)
                        {
                            sideMove = 0;
                        }
                        transform.position = new Vector3(maxRoadPosRight, transform.position.y, transform.position.z);
                    }
                    else if (transform.position.x >= maxRoadPosLeft)
                    {
                        if (sideMove < 0)
                        {
                            sideMove = 0;
                        }
                        transform.position = new Vector3(maxRoadPosLeft, transform.position.y, transform.position.z);
                    }
                }
                else if (movingDirection == 3) // left
                {
                    if (transform.position.z >= maxRoadPosRight)
                    {
                        if (sideMove > 0)
                        {
                            sideMove = 0;
                        }
                        transform.position = new Vector3(transform.position.x, transform.position.y, maxRoadPosRight);
                    }
                    else if (transform.position.z <= maxRoadPosLeft)
                    {
                        if (sideMove < 0)
                        {
                            sideMove = 0;
                        }
                        transform.position = new Vector3(transform.position.x, transform.position.y, maxRoadPosLeft);
                    }
                }
                mMovement.Move(transform.forward, sideMove);
            }
            else // on the rail
            {
                if ((targetPosToReach.position - transform.position).magnitude >= 0.1f)
                {
                    rBody.MovePosition(transform.position + (targetPosToReach.position - transform.position).normalized * Time.fixedDeltaTime * mMovement.GetSpeed());
                }
                else
                {
                    StartCoroutine(mMovement.SetSpeed(0f,0f));
                }
            }
        }
        else if (circularMove == 1)
        {
            if (currentAngle <= 1.6)
            {
                if (mMovement.GetSpeed() > 0f)
                {
                    currentAngle += circularMovementAngularSpeed * Time.fixedDeltaTime;
                    Vector3 offset = new Vector3(-1f * Mathf.Cos(currentAngle), 0f, 1f * Mathf.Sin(currentAngle)) * circularMovementRadius;
                    rBody.MovePosition(playerPosBeforeJump + offset + Vector3.right * circularMovementRadius);
                    transform.LookAt(playerPosBeforeJump + offset + Vector3.right * circularMovementRadius);
                    //transform.position = playerPosBeforeJump + offset + Vector3.forward * circleRad;
                }
            }
            else
            {
                grow.TurnWindVFX(false);
                currentAngle = 0;
                Debug.Log("Turning right finished");
                transform.rotation = Quaternion.Euler(0, 90, 0);
                circularMove = 0;
                canSwerve = true;
            }
        }
        else if (circularMove == 2) // turning from right to forward
        {
            if (currentAngle <= 1.6)
            {
                if (mMovement.GetSpeed() > 0f)
                {
                    currentAngle += circularMovementAngularSpeed * Time.fixedDeltaTime;
                    Vector3 offset = new Vector3(Mathf.Sin(currentAngle), 0f, -1f * Mathf.Cos(currentAngle)) * circularMovementRadius;
                    rBody.MovePosition(playerPosBeforeJump + offset + Vector3.forward * circularMovementRadius);
                    transform.LookAt(playerPosBeforeJump + offset + Vector3.forward * circularMovementRadius);

                    //transform.position = playerPosBeforeJump + offset + Vector3.forward * circleRad;
                }
            }
            else
            {
                grow.TurnWindVFX(false);
                currentAngle = 0;
                Debug.Log("Turning right to forward finished");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                circularMove = 0;
                canSwerve = true;
            }
        }
        else if (circularMove == 3) // turning from left to forward
        {
            if (currentAngle <= 1.6)
            {
                if (mMovement.GetSpeed() > 0f)
                {
                    currentAngle += circularMovementAngularSpeed * Time.fixedDeltaTime;
                    Vector3 offset = new Vector3(-1f * Mathf.Sin(currentAngle), 0f, -1f * Mathf.Cos(currentAngle)) * circularMovementRadius;
                    rBody.MovePosition(playerPosBeforeJump + offset + Vector3.forward * circularMovementRadius);
                    transform.LookAt(playerPosBeforeJump + offset + Vector3.forward * circularMovementRadius);
                }
            }
            else
            {
                grow.TurnWindVFX(false);
                currentAngle = 0;
                Debug.Log("Turning left to forward finished");
                transform.rotation = Quaternion.Euler(0, 0, 0);
                circularMove = 0;
                canSwerve = true;
            }
        }
        else if (circularMove == 4) // turning left
        {
            if (currentAngle <= 1.6)
            {
                if (mMovement.GetSpeed() > 0f)
                {
                    currentAngle += circularMovementAngularSpeed * Time.fixedDeltaTime;
                    Vector3 offset = new Vector3(1f * Mathf.Cos(currentAngle), 0f, 1f * Mathf.Sin(currentAngle)) * circularMovementRadius;
                    rBody.MovePosition(playerPosBeforeJump + offset + Vector3.left * circularMovementRadius);
                    transform.LookAt(playerPosBeforeJump + offset + Vector3.left * circularMovementRadius);
                }
            }
            else
            {
                grow.TurnWindVFX(false);
                currentAngle = 0;
                Debug.Log("Turning left finished");
                transform.rotation = Quaternion.Euler(0, -90, 0);
                circularMove = 0;
                canSwerve = true;
            }
        }
        else if (circularMove == 5) // turning right to back
        {
            if (currentAngle <= 1.6)
            {
                if (mMovement.GetSpeed() > 0f)
                {
                    currentAngle += circularMovementAngularSpeed * Time.fixedDeltaTime;
                    Vector3 offset = new Vector3(1f * Mathf.Sin(currentAngle), 0f, 1f * Mathf.Cos(currentAngle)) * circularMovementRadius;
                    rBody.MovePosition(playerPosBeforeJump + offset + Vector3.back * circularMovementRadius);
                    transform.LookAt(playerPosBeforeJump + offset + Vector3.back * circularMovementRadius);
                }
            }
            else
            {
                grow.TurnWindVFX(false);
                currentAngle = 0;
                Debug.Log("turning right to back finished");
                transform.rotation = Quaternion.Euler(0, 180, 0);
                circularMove = 0;
                canSwerve = true;
            }
        }
        else if (circularMove == 6) // turning back to right in world pos
        {
            if (currentAngle <= 1.6)
            {
                if (mMovement.GetSpeed() > 0f)
                {
                    currentAngle += circularMovementAngularSpeed * Time.fixedDeltaTime;
                    Vector3 offset = new Vector3(-1f * Mathf.Cos(currentAngle), 0f, -1f * Mathf.Sin(currentAngle)) * circularMovementRadius;
                    rBody.MovePosition(playerPosBeforeJump + offset + Vector3.right * circularMovementRadius);
                    transform.LookAt(playerPosBeforeJump + offset + Vector3.right * circularMovementRadius);
                }
            }
            else
            {
                grow.TurnWindVFX(false);
                currentAngle = 0;
                Debug.Log("turning back to right in world pos finished");
                transform.rotation = Quaternion.Euler(0, 90, 0);
                circularMove = 0;
                canSwerve = true;
            }
        }
        else if (circularMove == 7) // turning back to left in world space
        {
            if (currentAngle <= 1.6)
            {
                if (mMovement.GetSpeed() > 0f)
                {
                    currentAngle += circularMovementAngularSpeed * Time.fixedDeltaTime;
                    Vector3 offset = new Vector3(Mathf.Cos(currentAngle), 0f, -1f * Mathf.Sin(currentAngle)) * circularMovementRadius;
                    rBody.MovePosition(playerPosBeforeJump + offset + Vector3.left * circularMovementRadius);
                    transform.LookAt(playerPosBeforeJump + offset + Vector3.left * circularMovementRadius);
                }
            }
            else
            {
                grow.TurnWindVFX(false);
                currentAngle = 0;
                Debug.Log("Turning left to back finished");
                transform.rotation = Quaternion.Euler(0, -90, 0);
                circularMove = 0;
                canSwerve = true;
            }
        }
        else if (circularMove == 8) // turning left to back
        {
            if (currentAngle <= 1.6)
            {
                if (mMovement.GetSpeed() > 0f)
                {
                    currentAngle += circularMovementAngularSpeed * Time.fixedDeltaTime;
                    Vector3 offset = new Vector3(-1f * Mathf.Sin(currentAngle), 0f, 1f * Mathf.Cos(currentAngle)) * circularMovementRadius;
                    rBody.MovePosition(playerPosBeforeJump + offset + Vector3.back * circularMovementRadius);
                    transform.LookAt(playerPosBeforeJump + offset + Vector3.back * circularMovementRadius);
                }
            }
            else
            {
                grow.TurnWindVFX(false);
                currentAngle = 0;
                Debug.Log("Turning left to back finished");
                transform.rotation = Quaternion.Euler(0, 180, 0);
                circularMove = 0;
                canSwerve = true;
            }
        }
        else if (circularMove == 9) // jumping from trampoline
        {
            if (mMovement.GetSpeed() > 0f)
            {
                testTime += Time.fixedDeltaTime;
                float yOffset = (grow.transform.localScale.x - grow.minimumSnowScale) / 2f;
                Vector3 offset = new Vector3();
                if (movingDirection == 0) // forward, z-axis
                {
                    offset = new Vector3(transform.position.x, tramplinJumpCurve.Evaluate(testTime) + yOffset, transform.position.z + 1 * Time.fixedDeltaTime * mMovement.GetSpeed());
                }
                else if (movingDirection == 1) // right, x-axis
                {
                    offset = new Vector3(transform.position.x + 1 * Time.fixedDeltaTime * mMovement.GetSpeed(), tramplinJumpCurve.Evaluate(testTime) + yOffset, transform.position.z);
                }
                else if (movingDirection == 2) // back, -z-axis
                {
                    offset = new Vector3(transform.position.x, tramplinJumpCurve.Evaluate(testTime) + yOffset, transform.position.z - 1 * Time.fixedDeltaTime * mMovement.GetSpeed());
                }
                else if (movingDirection == 3) // left, -x-axis
                {
                    offset = new Vector3(transform.position.x - 1 * Time.fixedDeltaTime * mMovement.GetSpeed(), tramplinJumpCurve.Evaluate(testTime) + yOffset, transform.position.z);
                }
                rBody.MovePosition(offset);
            }
        }
        else if (circularMove == 10) // Ascend-Descend jump start
        {
            if (mMovement.GetSpeed() > 0f)
            {
                testTime += Time.fixedDeltaTime;
                float yOffset = (grow.transform.localScale.x - grow.minimumSnowScale) / 2f;
                Vector3 offset = new Vector3();
                if (movingDirection == 0) // forward, z-axis
                {
                    offset = new Vector3(transform.position.x, ascendDescendJumpCurve.Evaluate(testTime) + yOffset, transform.position.z + 1 * Time.fixedDeltaTime * mMovement.GetSpeed());
                }
                else if (movingDirection == 1) // right, x-axis
                {
                    offset = new Vector3(transform.position.x + 1 * Time.fixedDeltaTime * mMovement.GetSpeed(), ascendDescendJumpCurve.Evaluate(testTime) + yOffset, transform.position.z);
                }
                else if (movingDirection == 2) // back, -z-axis
                {
                    offset = new Vector3(transform.position.x, ascendDescendJumpCurve.Evaluate(testTime) + yOffset, transform.position.z - 1 * Time.fixedDeltaTime * mMovement.GetSpeed());
                }
                else if (movingDirection == 3) // left, -x-axis
                {
                    offset = new Vector3(transform.position.x - 1 * Time.fixedDeltaTime * mMovement.GetSpeed(), ascendDescendJumpCurve.Evaluate(testTime) + yOffset, transform.position.z);
                }
                rBody.MovePosition(offset);
            }
        }
        else if (circularMove == 11) // jumping after rail p1
        {
            if (currentAngle <= 1.3)
            {
                if (mMovement.GetSpeed() > 0f)
                {
                    testTime += Time.fixedDeltaTime;
                    float yOffset = (grow.transform.localScale.x - grow.minimumSnowScale) / 2f;
                    Vector3 offset = new Vector3();
                    if (movingDirection == 0) // forward, z-axis
                    {
                        offset = new Vector3(transform.position.x, afterRailCurve.Evaluate(testTime) + yOffset, transform.position.z + 1 * Time.fixedDeltaTime * mMovement.GetSpeed());
                    }
                    else if (movingDirection == 1) // right, x-axis
                    {
                        offset = new Vector3(transform.position.x + 1 * Time.fixedDeltaTime * mMovement.GetSpeed(), afterRailCurve.Evaluate(testTime) + yOffset, transform.position.z);
                    }
                    else if (movingDirection == 2) // back, -z-axis
                    {
                        offset = new Vector3(transform.position.x, afterRailCurve.Evaluate(testTime) + yOffset, transform.position.z - 1 * Time.fixedDeltaTime * mMovement.GetSpeed());
                    }
                    else if (movingDirection == 3) // left, -x-axis
                    {
                        offset = new Vector3(transform.position.x - 1 * Time.fixedDeltaTime * mMovement.GetSpeed(), afterRailCurve.Evaluate(testTime) + yOffset, transform.position.z);
                    }
                    rBody.MovePosition(offset);
                }
            }
        }

        if (mMovement.GetSpeed() > 0f)
        {
            humanCorpsParentTransform.Rotate(forwardRotationSpeed * Time.fixedDeltaTime, 0f, 0f);
            playerMeshTransform.Rotate(forwardRotationSpeed * Time.fixedDeltaTime, 0f, 0f);
        }
    }
}