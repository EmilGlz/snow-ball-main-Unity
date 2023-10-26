using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLogic : MonoBehaviour
{
    [SerializeField] float roadWidth;

    [Range(0f, 2f)]
    [SerializeField] float sideMoveSensitivity;

    [SerializeField] float forwardSpeed;

    private Rigidbody rBody;

    private Vector2 lastTouchPos;
    private Vector2 currentTouchPos;
    private float deltaX;
    private float sideMove;

    private float screenWidth;

    void Awake()
    {
        screenWidth = Screen.width;
        rBody = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        if(Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began)
            {
                lastTouchPos = touch.position;
            }
            // Move the cube if the screen has the finger moving.
            else if (touch.phase == TouchPhase.Moved)
            {
                currentTouchPos = touch.position;

                deltaX = lastTouchPos.x - currentTouchPos.x;

                sideMove = - deltaX / screenWidth * sideMoveSensitivity * roadWidth;

                lastTouchPos = currentTouchPos;
            }
            else if(touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Stationary)
            {
                sideMove = 0;
            }
        }
        //rBody.MoveRotation();
        rBody.MovePosition(transform.position + transform.forward * forwardSpeed + transform.right * sideMove);
    }
}
/*using UnityEngine;
 
 public class RaycastGround : MonoBehaviour
 {
     public float distance = 1.0f; // distance to raycast downwards (i.e. between transform.position and bottom of object)
     public LayerMask hitMask; // which layers to raycast against
 
     void Update()
     {
         Ray ray = new Ray(transform.position, Vector3.down);
         RaycastHit hit;
 
         Debug.DrawRay(ray.origin, ray.direction * distance, Color.red);
 
         if (Physics.Raycast(ray, out hit, distance, hitMask))
         {
             Debug.Log("Hit collider " + hit.collider + ", at " + hit.point + ", normal " + hit.normal);
             Debug.DrawRay(hit.point, hit.normal * 2f, Color.blue);
 
             float angle = Vector3.Angle(hit.normal, Vector3.up);
             Debug.Log("angle " + angle);
 
             //if (angle > 30)...
         }
         else // is not colliding
         {
             
         }
     }
 }

*/