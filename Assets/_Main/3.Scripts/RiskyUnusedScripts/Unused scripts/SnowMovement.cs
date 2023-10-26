using UnityEngine;
using UnityEngine.UI;


public class SnowMovement : MonoBehaviour
{
    [SerializeField] Text deltaXText;
    //[SerializeField] float horizontalTorque;
    [SerializeField] float horizontalSpeed;
    [SerializeField] float lerpTime;
    [SerializeField] Transform leftPos;
    [SerializeField] Transform rightPos;
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

    float deltaX;

    void FixedUpdate()
    {
        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began)
            {
                prevPos = t.position;
            }
            if (t.phase == TouchPhase.Moved)
            {
                currPos = t.position;
                deltaX = currPos.x - prevPos.x;

                bool swipedSideways = Mathf.Abs(deltaX) > 2.5f;
                if (swipedSideways)
                {
                    if (deltaX < 0)
                    {
                        //Debug.Log("Moving Left: " + deltaX);
                        rb.AddForce(transform.right * horizontalSpeed * Time.fixedDeltaTime * deltaX, ForceMode.Acceleration);
                        //player.transform.position = Vector3.Lerp(player.transform.position, leftPos.position , lerpTime * Time.fixedDeltaTime);
                    }
                    else if (deltaX > 0)
                    {
                        //Debug.Log("Moving Right: " + deltaX);
                        rb.AddForce(transform.right * horizontalSpeed * Time.fixedDeltaTime * deltaX, ForceMode.Acceleration);
                        //player.transform.position = Vector3.Lerp(player.transform.position, rightPos.position , lerpTime * Time.fixedDeltaTime);
                    }
                }
                prevPos = currPos;
            }
            if (t.phase == TouchPhase.Stationary)
            {
                deltaX = 0;
                Debug.Log("Touch Stationary");
            }
        }
        deltaXText.text = deltaX.ToString();

        if (Input.touchCount == 0)
        {
            Debug.Log("No touch");
            rb.velocity = Vector3.zero;
            //rb.AddTorque(transform.right * horizontalTorque);
            //rb.AddTorque(transform.right  * horizontalTorque * -1, ForceMode.VelocityChange);
        }

        if (player.position.x >= 2.4f)
        {
            player.position = new Vector3(2.4f, player.position.y, player.position.z);
        }
        else if (player.position.x <= -2.4f)
        {
            //player.transform.Translate(0.1f, 0, 0);
            player.position = new Vector3(-2.4f, player.position.y, player.position.z);
        }
    }
}