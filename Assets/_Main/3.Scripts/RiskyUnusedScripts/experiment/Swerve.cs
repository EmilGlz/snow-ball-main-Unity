using UnityEngine;

public class Swerve : MonoBehaviour
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
                float deltaX = currPos.x - prevPos.x;

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