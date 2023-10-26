using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SwerveMovementEL : MonoBehaviour
{
    private SwerveInputSystemEL _swerveInputSystem;
    private MovingSphereEL movingSphereEL;
    [SerializeField] Rigidbody rb;

    [SerializeField] Text speedText;
    //[SerializeField] float smoothFactor;

    [SerializeField] Text inputText;

    public enum MovementType { snowMovement, iceMovement };

    [SerializeField] MovementType movementType;

    [SerializeField] private float swerveSpeed = 0.5f;
    [SerializeField] private float horizontalForceInIce;
    public float currentNitroForce = 1;
    //[SerializeField] float nitroForce;
    [SerializeField] float nitroForceTime;
    //[SerializeField] private float maxSwerveAmount = 1f;
    [SerializeField] float maxSpeedAtNitro;
    [SerializeField] float rotationsSpeed;
    public float swerveAmountInSnow;
    public float swerveAmountInIce;
    float rotationAmount;

    [SerializeField] float swipeRange = 50;
    [HideInInspector] public bool swipedUp = false;
    Vector2 startTouchPos;
    Vector2 currentTouchPos;
    float normalMaxSpeed;


    private void Awake()
    {
        _swerveInputSystem = GetComponent<SwerveInputSystemEL>();
        movingSphereEL = GetComponent<MovingSphereEL>();
        normalMaxSpeed = movingSphereEL.maxSpeed;
    }

    public void SetMovementType(MovementType _movementType)
    {
        movementType = _movementType;
    }

    IEnumerator SetNitrovalue()
    {
        Debug.Log("currentnitro is max");
        movingSphereEL.maxSpeed = maxSpeedAtNitro;
        yield return new WaitForSeconds(nitroForceTime);
        Debug.Log("currentnitro is min");
        movingSphereEL.maxSpeed = normalMaxSpeed;
    }

    private void Update()
    {
        speedText.text = movingSphereEL.maxSpeed.ToString();
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                startTouchPos = t.position;
            }
            if (t.phase == TouchPhase.Moved)
            {
                currentTouchPos = t.position;
                Vector2 distance = currentTouchPos - startTouchPos;
                if (!swipedUp)
                {
                    if (distance.y > swipeRange && Mathf.Abs(distance.x) < 25)
                    {
                        swipedUp = true;
                        Debug.Log("Up");
                        if (movingSphereEL.maxSpeed == normalMaxSpeed) // if nitro is off
                        {
                            //StartCoroutine( SetNitrovalue() );
                        }
                    }
                }
            }
            if (t.phase == TouchPhase.Ended)
            {
                swipedUp = false;
            }
        }



        swerveAmountInSnow = Time.deltaTime * _swerveInputSystem.MoveFactorX;
        rotationAmount = swerveAmountInSnow;
        swerveAmountInIce = swerveAmountInSnow;

        swerveAmountInIce *= horizontalForceInIce;
        swerveAmountInSnow *= swerveSpeed;
        rotationAmount *= rotationsSpeed;
    }
    Vector3 velocity = Vector3.zero;
    private void FixedUpdate()
    {
        inputText.text = (swerveAmountInSnow * Time.fixedDeltaTime).ToString();

        if (movementType == MovementType.iceMovement) // ice movement, only add torque
        {
            rb.AddTorque(-1f * Vector3.forward * rotationAmount * Time.fixedDeltaTime);
            rb.AddForce(Vector3.right * swerveAmountInIce * Time.fixedDeltaTime);
        }
        else if (movementType == MovementType.snowMovement)
        {
            rb.AddTorque(-1f * Vector3.forward * rotationAmount * Time.fixedDeltaTime);
            rb.AddForce(Vector3.right * swerveAmountInSnow * Time.fixedDeltaTime);
            // ------------------------------------------Position = ,Lerp------------------------------------------------------------------------------------------------------------------------
            //if (swerveAmount > 0.2f) // right
            //{
            //    targetPosRight.x = 2.4f;
            //    targetPosRight.y = transform.position.y;
            //    targetPosRight.z = transform.position.z;
            //    transform.position = Vector3.Lerp(transform.position, targetPosRight, Time.fixedDeltaTime * smoothFactor);
            //}
            //if (swerveAmount < -0.2f) // left
            //{
            //    targetPosLeft.x = -2.4f;
            //    targetPosLeft.y = transform.position.y;
            //    targetPosLeft.z = transform.position.z;
            //    transform.position = Vector3.Lerp(transform.position, targetPosLeft, Time.fixedDeltaTime * smoothFactor);
            //}

            // ------------------------------------------Transform.Translate------------------------------------------------------------------------------------------------------------------------
            //transform.Translate(Vector3.right * swerveAmount * Time.fixedDeltaTime, Space.World);
            //rb.AddTorque(-1f * Vector3.forward * rotationAmount * Time.fixedDeltaTime);
            //rb.AddTorque(-1f * Vector3.forward * rotationAmount * Time.fixedDeltaTime);
            //rb.AddForce(Vector3.right * swerveAmount * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }

        //if (swipedUp) // weiped up, nitro 
        //{
        //    rb.AddForce(Vector3.forward * currentNitroForce * Time.fixedDeltaTime);
        //}

        // ------------------------------------------AddForce--------------------------------------------------------------------------------------------------------------------------------
        // servespeed => 5000
        //if (swerveAmount > 15f) // right
        //{
        //    rb.AddForce(Vector3.right * swerveAmount * Time.fixedDeltaTime);
        //}
        //if (swerveAmount < -15f) // left
        //{
        //    rb.AddForce(Vector3.right * swerveAmount * Time.fixedDeltaTime);
        //}

        // ------------------------------------------Position = ,Lerp------------------------------------------------------------------------------------------------------------------------
        // Position = , Lerp
        //if (swerveAmount > 0.2f) // right
        //{
        //    transform.position = Vector3.Lerp(transform.position, targetPosRight, Time.fixedDeltaTime * smoothFactor);
        //}
        //if (swerveAmount < -0.2f) // left
        //{
        //    transform.position = Vector3.Lerp(transform.position, targetPosLeft, Time.fixedDeltaTime * smoothFactor);
        //}

        // ------------------------------------------AddTorque (For lake) -------------------------------------------------------------------------------------------------------------------, smooth, but sometimes hard to comeback. But still good to use with lake, as it is rotating like real
        //if (Mathf.Abs(swerveAmount) > 200f)
        //{
        //    rb.AddTorque(-1f * Vector3.forward * swerveAmount * Time.fixedDeltaTime);
        //}

        // ------------------------------------------Position = ,SmoothDamp------------------------------------------------------------------------------------------------------------------, not good
        //if (swerveAmount > 1) // right
        //{
        //    targetPosRight.x = 2.4f;
        //    targetPosRight.y = transform.position.y;
        //    targetPosRight.z = transform.position.z;
        //    transform.position = Vector3.SmoothDamp(transform.position, targetPosRight, ref velocity, Time.fixedDeltaTime * smoothFactor);
        //}
        //if (swerveAmount < -1) // left
        //{
        //    targetPosLeft.x = -2.4f;
        //    targetPosLeft.y = transform.position.y;
        //    targetPosLeft.z = transform.position.z;
        //    transform.position = Vector3.SmoothDamp(transform.position, targetPosLeft, ref velocity, Time.fixedDeltaTime * smoothFactor);
        //}



    }


    public void Test()
    {
        rb.AddForce(transform.right * -1f * swerveAmountInSnow, ForceMode.Acceleration);
        //LeanTween.moveX(gameObject, 2.4f, 1f); 
    }


}